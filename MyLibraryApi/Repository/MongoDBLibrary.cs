
using MongoDB.Bson;
using MongoDB.Driver;
using MyLibraryApi.Entities;
using MyLibraryApi.Repository;
namespace MyLibraryApi.Repositories {
    public class MongoDbItemsRepository: IBookRepository {

        private const string databaseName = "Library";
        private const string collectionName = "Books";
        private readonly IMongoCollection<Book> booksCollection;
        private readonly FilterDefinitionBuilder<Book> filterBuilder = Builders<Book>.Filter;
        public MongoDbItemsRepository(IMongoClient mongoClient)
        {
            IMongoDatabase database = mongoClient.GetDatabase(databaseName);
            booksCollection = database.GetCollection<Book>(collectionName);
        }

        public async Task CreateBookAsync(Book book)
        {
            await booksCollection.InsertOneAsync(book);
        }

        public async Task DeleteBookAsync(Book book)
        
        {
            var filter = filterBuilder.Eq(book => book.Id, book.Id);
            await booksCollection.DeleteOneAsync(filter);
        }

        public async Task<Book> GetBookAsync(Guid id)
        {
            var filter = filterBuilder.Eq(book => book.Id, id);
            return await booksCollection.Find(filter).SingleOrDefaultAsync();
        }

        public async Task UpdateBookAsync(Book book)
        {
            var filter = filterBuilder.Eq(existingBook => existingBook.Id, book.Id);
            await booksCollection.ReplaceOneAsync(filter, book);
        }

        public async Task<IEnumerable<Book>> GetBooksAsync()
        {
            return await booksCollection.Find(new BsonDocument()).ToListAsync();
        }

    }
}