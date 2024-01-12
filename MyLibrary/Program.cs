using System.Net.Mime;
using System.Text.Json;
using Microsoft.AspNetCore.Diagnostics.HealthChecks;
using Microsoft.Extensions.Options;
using MongoDB.Bson;
using MongoDB.Bson.Serialization;
using MongoDB.Bson.Serialization.Serializers;
using MongoDB.Driver;
using MyLibrary.Repositories;
using MyLibrary.Repository;
using MyLibrary.Settings;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
//builder.Services.AddSingleton<IBookRepository, BookRepository>();

BsonSerializer.RegisterSerializer(new GuidSerializer(BsonType.String)); // necessary for mongodb id 
BsonSerializer.RegisterSerializer(new DateTimeOffsetSerializer(BsonType.String)); // necessary for created date mongo

builder.Services.Configure<MongoDbSettings>(builder.Configuration.GetSection("MongoDbSettings"));
builder.Services.AddSingleton<IMongoClient>(serviceProvider =>  
{
    var settings = serviceProvider.GetRequiredService<IOptions<MongoDbSettings>>().Value;
    return new MongoClient(settings.ConnectionString);
});
builder.Services.AddSingleton<IBookRepository, MongoDbItemsRepository>(); // because of this, you are using the mongo db instead of the BookRepository
builder.Services.AddControllers(options => 
{
    options.SuppressAsyncSuffixInActionNames = false;
}); // helps to solve the problem for async in GetItemasync
builder.Services.AddHealthChecks()
    .AddMongoDb(
        settings => settings.GetRequiredService<IOptions<MongoDbSettings>>().Value.ConnectionString,
        name: "mongodb", timeout: TimeSpan.FromSeconds(3),
        tags: new[]{"ready"});
var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseRouting();
app.UseAuthorization();
app.UseEndpoints(endpoints =>
{
    endpoints.MapControllers();

    endpoints.MapHealthChecks("/health/ready", new HealthCheckOptions{
        Predicate = (check) => check.Tags.Contains("ready"), // make sure the db is actually working
    }); 
    
    endpoints.MapHealthChecks("/health/live", new HealthCheckOptions{
        Predicate = (_) => false // response from a ping 
    }); 
});


app.MapControllers();

app.Run();
