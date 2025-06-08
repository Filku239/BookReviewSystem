namespace BookSystem.Models
{
    public class Users
    {
        public int Id { get; set; }
        public required string Username { get; set; }
        public required string HashedPassword { get; set; }
        public required string Email { get; set; }
    }
}