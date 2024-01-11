
using MongoDB.Driver;
using MyLibrary.Entities;
using MyLibrary.Repository;
namespace MYWEBAPI.Repositories {
    public class MongoDbItemsRepository: IBookRepository {

        private const string databaseName = "Library";
        private const string collectionName = "Books";
        private readonly IMongoCollection<Book> booksCollection;
        public MongoDbItemsRepository(IMongoClient mongoClient)
        {
            IMongoDatabase database = mongoClient.GetDatabase(databaseName);
            booksCollection = database.GetCollection<Book>(collectionName);
        }

        public void CreateBook(Book book)
        {
            booksCollection.InsertOne(book);
        }

        public void DeleteBook(Book book)
        
        {
            throw new NotImplementedException();
        }

        public Book GetBook(Guid id)
        {
            throw new NotImplementedException();
        }

        public void UpdateBook(Book book)
        {
            throw new NotImplementedException();
        }

        public IEnumerable<Book> GetBooks()
        {
            throw new NotImplementedException();
        }

    }
}