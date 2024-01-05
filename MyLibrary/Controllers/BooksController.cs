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

        public BooksController(IBookRepository repository) { // this class depends on the interface (dependency injection)
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

        [HttpPost]
        public ActionResult<BookDTO> CreateBook(CreateBookDTO bookDTO){
            Book book = new ()
            {
                Id = Guid.NewGuid(),
                Name = bookDTO.Name,
                Author = bookDTO.Author,
                Rating = bookDTO.Rating,
                CreatedDate = DateTimeOffset.UtcNow
            };
            repository.CreateBook(book);
            return CreatedAtAction(nameof(GetBook), new { id = book.Id }, book.AsDto());
        }

        [HttpPut]
        public ActionResult<BookDTO> UpdateBook(Guid id, UpdateBookDTO bookDTO){
            var existingBook = repository.GetBook(id);
            if (existingBook is null){
                return NotFound();
            }
            Book updatedBook = existingBook with {
                Name = bookDTO.Name,
                Author = bookDTO.Author,
                Rating = bookDTO.Rating
            };
            repository.UpdateBook(updatedBook);
            return NoContent();
        }

        [HttpDelete("{id}")]
        public ActionResult<BookDTO> DeleteBook(Guid id){
            var existingBook = repository.GetBook(id);
            
            if (existingBook is null){
                return NotFound();
            }
            repository.DeleteBook(existingBook);
            return NoContent();
        }
    }
}
