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
        public async Task<IEnumerable<BookDTO>> GetBooksAsync(){
            var books = (await repository.GetBooksAsync()).Select(book => book.AsDto());
            return books;
        }

        // Get /book/{id}
        [HttpGet("{id}")]
        public async Task <ActionResult<BookDTO>> GetBookAsync(Guid id){
            var book = await repository.GetBookAsync(id);
            if (book is null){
                return NotFound(); // You need to use ActionResult to return an item type
            }
            return book.AsDto();
        }

        [HttpPost]
        public async Task<ActionResult<BookDTO>> CreateBookAsync(CreateBookDTO bookDTO){
            Book book = new ()
            {
                Id = Guid.NewGuid(),
                Name = bookDTO.Name,
                Author = bookDTO.Author,
                Rating = bookDTO.Rating,
                CreatedDate = DateTimeOffset.UtcNow
            };
            await repository.CreateBookAsync(book);
            return CreatedAtAction(nameof(GetBookAsync), new { id = book.Id }, book.AsDto());
        }

        [HttpPut]
        public async Task<ActionResult<BookDTO>> UpdateBookAsync(Guid id, UpdateBookDTO bookDTO){
            var existingBook = await repository.GetBookAsync(id);
            if (existingBook is null){
                return NotFound();
            }
            Book updatedBook = existingBook with {
                Name = bookDTO.Name,
                Author = bookDTO.Author,
                Rating = bookDTO.Rating
            };
            await repository.UpdateBookAsync(updatedBook);
            return NoContent();
        }

        [HttpDelete("{id}")]
        public async Task<ActionResult<BookDTO>> DeleteBookAsync(Guid id){
            var existingBook = await repository.GetBookAsync(id);
            
            if (existingBook is null){
                return NotFound();
            }
            await repository.DeleteBookAsync(existingBook);
            return NoContent();
        }
    }
}
