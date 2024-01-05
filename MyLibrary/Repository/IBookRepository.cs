using MyLibrary.Entities;

namespace MyLibrary.Repository {
    public interface IBookRepository {
        Book GetBook(Guid id);
        IEnumerable<Book> GetBooks();
        void CreateBook(Book book);
        void UpdateBook(Book book);
        void DeleteBook(Book book);
    }
}
