namespace MyLibrary.Entities
{
    public record Book
    {
        public Guid Id { get; set; }
        public string Name {get; init;}
        public string Author {get; init;}
        public decimal Rating {get; init;}
        public DateTimeOffset CreatedDate {get; init;}
    }
}
