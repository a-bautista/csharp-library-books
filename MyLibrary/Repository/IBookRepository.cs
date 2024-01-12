using MyLibrary.Entities;
using System.Threading.Tasks;

namespace MyLibrary.Repository {
    public interface IBookRepository {
        Task <Book> GetBookAsync(Guid id);
        Task <IEnumerable<Book>> GetBooksAsync();
        Task CreateBookAsync(Book book);
        Task UpdateBookAsync(Book book);
        Task DeleteBookAsync(Book book);
    }
}
