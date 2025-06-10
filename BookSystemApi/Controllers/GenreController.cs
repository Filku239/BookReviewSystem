using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using BookSystem.Data;
using BookSystem.Models;

namespace BookSystem.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class GenreController : ControllerBase
    {
        private readonly AppDbContext _context;

        public GenreController(AppDbContext context)
        {
            _context = context;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<Genre>>> GetGenres()
        {
            return await _context.Genres.ToListAsync();
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<Genre>> GetGenre(int id)
        {
            var genre = await _context.Genres.FindAsync(id);

            if (genre == null)
            {
                return NotFound();
            }

            return genre;
        }

        [HttpPost]
        public async Task<ActionResult<Genre>> PostGenre(Genre genre)
        {
            _context.Genres.Add(genre);
            await _context.SaveChangesAsync();

            return CreatedAtAction(nameof(GetGenre), new { id = genre.Id }, genre);
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteGenre(int id)
        {
            var genre = await _context.Genres.FindAsync(id);
            if (genre == null)
            {
                return NotFound();
            }

            _context.Genres.Remove(genre);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        [HttpGet("count")]
        public async Task<ActionResult<int>> GetGenreCount()
        {
            try
            {
                int count = await _context.Books.CountAsync();
                return Ok(count);
            }
             catch (Exception ex)
            {
                Console.WriteLine($"Error retrieving genre count: {ex.Message}"); 
                return StatusCode(500, "An internal server error occurred while retrieving the book count.");
            }
        }
    }
}
