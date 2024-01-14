namespace MyLibraryApi.Entities
{
    public class Book
    {
        public Guid Id { get; set; }
        public string Name {get; set;}
        public string Author {get; set;}
        public decimal Rating {get; set;}
        public DateTimeOffset CreatedDate {get; set;}
    }
}
