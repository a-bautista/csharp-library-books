using MyLibraryApi.DTO;
using MyLibraryApi.Entities;

namespace MyLibraryApi {
    public static class Extensions // extensions are static
    {
        public static BookDTO AsDto(this Book book)
        {
            return new BookDTO(book.Id, book.Name, book.Author, book.Rating, book.CreatedDate);
        }
    }
}