using System;
using System.ComponentModel.DataAnnotations;

namespace MyLibraryApi.DTO {

    public record BookDTO(Guid Id, string Name, string Author, decimal Rating, DateTimeOffset CreatedDate);
    public record CreateBookDTO([Required] string Name, [Required] string Author, [Range(1,5)] decimal Rating);
    public record UpdateBookDTO([Required] string Name, [Required] string Author, [Range(1,5)] decimal Rating);
}