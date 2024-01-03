using Microsoft.AspNetCore.Mvc;
using MyLibrary.DTO;
using MyLibrary.Entities;
using MyLibrary.Repository;

namespace MyLibrary.Controllers
{
    [ApiController]
    [Route("books")] // Get books
    public class BooksController : ControllerBase
    {
        private readonly IBookRepository repository;

        public BooksController(IBookRepository repository) {
            this.repository = repository;
        }

        [HttpGet] // Get /books
        public IEnumerable<BookDTO> GetBooks(){
            var books = repository.GetBooks().Select(book => new BookDTO{
                Id = book.Id,
                Name = book.Name,
                Author = book.Author
            });
            return books;
        }

        // Get /book/{id}
        [HttpGet("{id}")]
        public ActionResult<Book> GetBook(Guid id){
            var item = repository.GetBook(id);
            if (item is null){
                return NotFound(); // You need to use ActionResult to return an item type
            }
            return item;
        }
    }
}
