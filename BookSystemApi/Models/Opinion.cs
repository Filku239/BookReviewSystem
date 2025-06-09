namespace BookSystem.Models
{
    public class Opinion
    {
        public int Id { get; set; }
        public string Text { get; set; } = string.Empty;
        public DateOnly opinionDate { get; set; }
        public int UserId { get; set; }
        public int BookId { get; set; }
    }
}
