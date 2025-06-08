using System.Text.Json.Serialization;

namespace BookSystem.Models
{
    public class Genre
    {
        public int Id { get; set; }
        public required string Name { get; set; }

        [JsonIgnore]

        public ICollection<Book>? Books { get; set; }
    }
}
