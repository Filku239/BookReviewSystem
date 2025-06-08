namespace BookSystem.Models
{
    public class Book
    {
        public int Id { get; set; }
        public required string Title { get; set; }
        public required string Author { get; set; }
        public decimal Price { get; set; }

        public DateTime ReleaseDate { get; set; }

        //Foreign key
        public int GenreId { get; set; }
    }
}