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
            var books = repository.GetBooks().Select(book => book.AsDto());
            return books;
        }

        // Get /book/{id}
        [HttpGet("{id}")]
        public ActionResult<BookDTO> GetBook(Guid id){
            var book = repository.GetBook(id);
            if (book is null){
                return NotFound(); // You need to use ActionResult to return an item type
            }
            return book.AsDto();
        }
    }
}
