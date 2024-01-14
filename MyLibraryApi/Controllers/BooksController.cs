using Microsoft.AspNetCore.Mvc;
using MyLibraryApi.DTO;
using MyLibraryApi.Entities;
using MyLibraryApi.Repository;

namespace MyLibraryApi.Controllers
{
    [ApiController]
    [Route("books")] // Get books
    public class BooksController : ControllerBase
    {
        private readonly IBookRepository repository;
        private readonly ILogger<BooksController> logger;

        public BooksController(IBookRepository repository, ILogger<BooksController> logger)
        { // this class depends on the interface (dependency injection)
            this.repository = repository;
            this.logger = logger;
        }

        [HttpGet] // Get /books
        public async Task<IEnumerable<BookDTO>> GetBooksAsync(string nameToMatch = null)
        {
            var books = (await repository.GetBooksAsync()).Select(book => book.AsDto());

            if (!string.IsNullOrWhiteSpace(nameToMatch))
            {
                books = books.Where(book => book.Author.Contains(nameToMatch, StringComparison.OrdinalIgnoreCase));
            }

            logger.LogInformation($"{DateTime.UtcNow:hh:mm:ss}: Retrieved {books.Count()} books");
            return books;
        }

        // Get /book/{id}
        [HttpGet("{id}")]
        public async Task<ActionResult<BookDTO>> GetBookAsync(Guid id)
        {
            var book = await repository.GetBookAsync(id);
            if (book is null)
            {
                return NotFound(); // You need to use ActionResult to return an item type
            }
            return book.AsDto();
        }

        [HttpPost]
        public async Task<ActionResult<BookDTO>> CreateBookAsync(CreateBookDTO bookDTO)
        {
            Book book = new()
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

        [HttpPut("{id}")]
        public async Task<ActionResult<BookDTO>> UpdateBookAsync(Guid id, UpdateBookDTO bookDTO)
        {
            var existingBook = await repository.GetBookAsync(id);
            if (existingBook is null)
            {
                return NotFound();
            }

            existingBook.Name = bookDTO.Name;
            existingBook.Author = bookDTO.Author;
            existingBook.Rating = bookDTO.Rating;

            await repository.UpdateBookAsync(existingBook);
            return NoContent();
        }

        [HttpDelete("{id}")]
        public async Task<ActionResult<BookDTO>> DeleteBookAsync(Guid id)
        {
            var existingBook = await repository.GetBookAsync(id);

            if (existingBook is null)
            {
                return NotFound();
            }
            await repository.DeleteBookAsync(existingBook);
            return NoContent();
        }
    }
}
