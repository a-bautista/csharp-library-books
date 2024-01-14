using System.ComponentModel.DataAnnotations;

namespace MyLibraryApi.DTO {
    public record UpdateBookDTO {
        [Required]
        public string Name {get; init;}
        [Required]
        public string Author {get; init;}
        [Required]
        [Range(1,5)]
        public decimal Rating {get; init;}        
    }
}