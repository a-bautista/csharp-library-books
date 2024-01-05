using System.Linq;
using MyLibrary.Entities;
namespace MyLibrary.Repository {

    public class BookRepository: IBookRepository // BookRepository implements the interface
    {
        private readonly List<Book> books = new()
        {
            new Book {Id = Guid.NewGuid(), Name = "Taiko I", Author = "Eiji Yoshikawa", Rating=4.5M, CreatedDate = DateTimeOffset.UtcNow},
            new Book {Id = Guid.NewGuid(), Name = "Taiko II", Author = "Eiji Yoshikawa", Rating=4, CreatedDate = DateTimeOffset.UtcNow}
        };

        public IEnumerable<Book> GetBooks(){ // basic interface IEnumerable to get the books
            return books;
        } 

        public Book GetBook(Guid id){
            return books.Where(books => books.Id == id).SingleOrDefault();
        }
        public void CreateBook(Book book){
            books.Add(book);
        }

        public void UpdateBook(Book book){
            var index = books.FindIndex(existingBook => existingBook.Id == book.Id);
            books[index] = book;
        }
        
        public void DeleteBook(Book book){
            var index = books.FindIndex(existingBook => existingBook.Id == book.Id);
            books.RemoveAt(index);
        }
    }
}