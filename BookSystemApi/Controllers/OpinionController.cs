using Microsoft.AspNetCore.Mvc;
using BookSystem.Models;
using BookSystem.Data;
using Microsoft.EntityFrameworkCore;

namespace BookSystemApi.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class OpinionController : ControllerBase
    {
        private readonly AppDbContext _context;

        public OpinionController(AppDbContext context)
        {
            _context = context;
        }

        [HttpGet("user/{userId}")]
        public async Task<ActionResult<Opinion>> GetOpinionByUser(int userId)
        {
            var opinions = await _context.Opinions.Where(o => o.UserId == userId).ToListAsync();

            if (opinions == null)
            {
                return NotFound();
            }

            return Ok(opinions);
        }

        [HttpGet("book/{bookId}")]
        public async Task<ActionResult<Opinion>> GetOpinionByBooks(int bookId)
        {
            var opinions = await _context.Opinions.Where(o => o.BookId == bookId).ToListAsync();

            if (opinions == null)
            {
                return NotFound();
            }

            return Ok(opinions);
        }

        [HttpPost]
        public async Task<ActionResult<Opinion>> PostOpinion(Opinion opinion)
        {
            _context.Opinions.Add(opinion);
            await _context.SaveChangesAsync();

            return CreatedAtAction(nameof(GetOpinionByBooks), new { id = opinion.Id }, opinion);
        }

        [HttpGet("count")]
        public async Task<ActionResult<int>> GetOpinionCount()
        {
            try
            {
                int count = await _context.Opinions.CountAsync();
                return Ok(count);
            }
             catch (Exception ex)
            {
                Console.WriteLine($"Error retrieving opinion count: {ex.Message}"); 
                return StatusCode(500, "An internal server error occurred while retrieving the book count.");
            }
        }
    }
}
