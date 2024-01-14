namespace MyLibraryApi.DTO {
    public record BookDTO {
        public Guid Id { get; set; }
        public string Name {get; init;}
        public string Author {get; init;}
        public decimal Rating {get; init;}
        public DateTimeOffset CreatedDate {get; init;}
    }
}