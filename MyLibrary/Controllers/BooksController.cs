using Microsoft.AspNetCore.Mvc;
using MyLibrary.Entities;
using MyLibrary.Repository;

namespace MyLibrary.Controllers
{
    [ApiController]
    [Route("books")] // Get books
    public class BooksController : ControllerBase
    {
        private readonly BookRepository repository;

        public BooksController() {
            repository = new BookRepository();
        }

        [HttpGet] // Get /books
        public IEnumerable<Book> GetBooks(){
            var books = repository.GetBooks();
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
