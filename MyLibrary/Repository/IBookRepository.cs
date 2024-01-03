using MyLibrary.Entities;

namespace MyLibrary.Repository {
    public interface IBookRepository {
        Book GetBook(Guid id);
        IEnumerable<Book> GetBooks();
    }
}
