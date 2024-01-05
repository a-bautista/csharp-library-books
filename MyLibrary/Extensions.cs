using MyLibrary.DTO;
using MyLibrary.Entities;

namespace MyLibrary {
    public static class Extensions // extensions are static
    {
        public static BookDTO AsDto(this Book book)
        {
            return new BookDTO
            {
                Id = book.Id,
                Name = book.Name,
                Author = book.Author,
                Rating = book.Rating,
                CreatedDate = book.CreatedDate
            };
        }
    }
}