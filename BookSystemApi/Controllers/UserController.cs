using BookSystem.Data;
using BookSystem.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using System;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace BookSystem.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UserController : ControllerBase
    {
        private readonly AppDbContext _context;

        public UserController(AppDbContext context)
        {
            _context = context;
        }

        [HttpPost("login")]
        public async Task<IActionResult> Login([FromBody] Users user)
        {
            var hashedInputPassword = HashPassword(user.HashedPassword);

            var userLogin = await _context.Users
                .FirstOrDefaultAsync(x => x.Email == user.Email && x.HashedPassword == hashedInputPassword);

            if (userLogin != null)
            {
                var token = GenerateToken(userLogin);
                return Ok(new { token });
            }

            return BadRequest("Invalid credentials");
        }

        [HttpPost("register")]
        public async Task<IActionResult> Register([FromBody] Users user)
        {
            if (await _context.Users.AnyAsync(u => u.Email == user.Email))
                return BadRequest("User with this email already exists.");

            user.HashedPassword = HashPassword(user.HashedPassword);

            _context.Users.Add(user);
            await _context.SaveChangesAsync();

            return CreatedAtAction(nameof(GetUser), new { id = user.Id }, new { user.Id, user.Email });
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetUser(int id)
        {
            var user = await _context.Users.FindAsync(id);

            if (user == null)
                return NotFound();

            return Ok(new { user.Id, user.Email });
        }

        private string HashPassword(string password)
        {
            using var sha = SHA256.Create();
            var bytes = Encoding.UTF8.GetBytes(password);
            var hash = sha.ComputeHash(bytes);
            return Convert.ToBase64String(hash);
        }

        private string GenerateToken(Users user)
        {
            var jwtSecret = Environment.GetEnvironmentVariable("JWT_SECRET");
            if (string.IsNullOrEmpty(jwtSecret))
                throw new Exception("JWT_SECRET is not set in the environment.");

            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(jwtSecret));
            var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

            var claims = new[]
            {
                new Claim(ClaimTypes.NameIdentifier, user.Id.ToString()),
                new Claim(ClaimTypes.Name, user.Email)
            };

            var token = new JwtSecurityToken(
                issuer: "http://localhost",
                audience: "http://localhost",
                claims: claims,
                expires: DateTime.UtcNow.AddHours(1),
                signingCredentials: creds
            );

            return new JwtSecurityTokenHandler().WriteToken(token);
        }
    }
}
