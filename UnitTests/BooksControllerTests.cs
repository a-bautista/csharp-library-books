using MyLibraryApi.Controllers;
using MyLibraryApi.Entities;
using MyLibraryApi.Repository;
using MyLibraryApi.DTO;
using Xunit;
using Moq;
using Microsoft.Extensions.Logging;
using Microsoft.AspNetCore.Mvc;
using FluentAssertions;

namespace MyLibraryApi.UnitTests
{
    public class BooksControllerTests
    {
        private readonly Mock<IBookRepository> repositoryStub = new();

        private readonly Mock<ILogger<BooksController>> loggerStub = new();
        private readonly Random random = new();

        [Fact]
        public async void GetBookAsync_WithUnexistingBook_ReturnsNotFound()
        {
            // Arrange
            
            repositoryStub.Setup(repo => repo.GetBookAsync(It.IsAny<Guid>())).ReturnsAsync((Book)null);

            
            var controller = new BooksController(repositoryStub.Object, loggerStub.Object);

            // Act
            var result = await controller.GetBookAsync(Guid.NewGuid());

            // Assert
            //Assert.IsType<NotFoundResult>(result.Result);
            result.Result.Should().BeOfType<NotFoundResult>();
        }

        [Fact]
        public async void GetBookAsync_WithExistingBook_ReturnsExpectedBook()
        {

            // Arrange            
            var expectedBook = CreateRandomBook();
            repositoryStub.Setup(repo => repo.GetBookAsync(It.IsAny<Guid>())).ReturnsAsync(expectedBook);

            var controller = new BooksController(repositoryStub.Object, loggerStub.Object);

            // Act
            var result = await controller.GetBookAsync(Guid.NewGuid());

            // Assert
            result.Value.Should().BeEquivalentTo(
                expectedBook, 
                options => options.ComparingByMembers<Book>());
        }

        [Fact]
        public async void GetBookAsync_WithExistingBook_ReturnsAllBooks()
        {

            // Arrange            
            var expectedBooks = new[] {CreateRandomBook(), CreateRandomBook(), CreateRandomBook()};

            repositoryStub.Setup(repo => repo.GetBooksAsync()).ReturnsAsync(expectedBooks);

            var controller = new BooksController(repositoryStub.Object, loggerStub.Object);

            // Act
            var actualBooks = await controller.GetBooksAsync();

            // Assert
            actualBooks.Should().BeEquivalentTo(
                expectedBooks, 
                options => options.ComparingByMembers<Book>());   
        }
        [Fact]
        public async Task CreateBookAsync_WithBookToCreate_ReturnsCreatedBook()
        {
            // Arrange
            var bookToCreate = new CreateBookDTO(
                Guid.NewGuid().ToString(), 
                Guid.NewGuid().ToString(), 
                random.Next(5));

            var controller = new BooksController(repositoryStub.Object, loggerStub.Object);

            // Act
            var result = await controller.CreateBookAsync(bookToCreate);

            // Assert
            var createdBook = (result.Result as CreatedAtActionResult).Value as BookDTO;
            bookToCreate.Should().BeEquivalentTo(
                createdBook,
                // Use this because you need to compare for the CreateBookDTO
                options => options.ComparingByMembers<BookDTO>().ExcludingMissingMembers()
            );

            // the missing properties which are ID and date
            //createdBook.Id.Should().NotBeEmpty();
            createdBook.CreatedDate.Should().BeCloseTo(DateTimeOffset.UtcNow, TimeSpan.FromMilliseconds(1000));
        }

        [Fact]
        public async Task UpdateBookAsync_WithExistingBook_ReturnsNoContent()
        {
            // Arrange
            var existingBook = CreateRandomBook();
            repositoryStub.Setup(repo => repo.GetBookAsync(It.IsAny<Guid>())).ReturnsAsync(existingBook);

            var bookId = existingBook.Id;
            var bookToUpdate = new UpdateBookDTO(Guid.NewGuid().ToString(), Guid.NewGuid().ToString(), random.Next(1))
            {
                Name = Guid.NewGuid().ToString(),
                Author = Guid.NewGuid().ToString(),
                Rating = random.Next(1)
            };
            var controller = new BooksController(repositoryStub.Object, loggerStub.Object);
            // ACT
            var result = await controller.UpdateBookAsync(bookId, bookToUpdate);

            // Assert
            result.Result.Should().BeOfType<NoContentResult>();
        }

        [Fact]
        public async Task DeleteBookAsync_WithExistingBook_ReturnsNoContent()
        {
            // Arrange
            var existingBook = CreateRandomBook();
            repositoryStub.Setup(repo => repo.GetBookAsync(It.IsAny<Guid>())).ReturnsAsync(existingBook);

            var controller = new BooksController(repositoryStub.Object, loggerStub.Object);
            // ACT
            var result = await controller.DeleteBookAsync(existingBook.Id);

            // Assert
            result.Result.Should().BeOfType<NoContentResult>();
        }

        private Book CreateRandomBook()
        {
            return new ()
            {
                Id = Guid.NewGuid(),
                Name = Guid.NewGuid().ToString(),
                Author = Guid.NewGuid().ToString(),
                Rating = random.Next(5),
                CreatedDate = DateTimeOffset.UtcNow
            };
        }
    }
}