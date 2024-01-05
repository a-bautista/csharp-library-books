using System.ComponentModel.DataAnnotations;

namespace MyLibrary.DTO {
    public record CreateBookDTO {
        [Required]
        public string Name {get; init;}
        [Required]
        public string Author {get; init;}
        [Required]
        [Range(1,5)]
        public decimal Rating {get; init;}

        // you don't need the date of creation and the guid when you create an item
        // this is the purpose of the DTO, the contract between the client and server to show what
        // is useful for the client
        
    }
}