using ComixAPIWebApp.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace ComixAPIWebApp.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class GenresController : ControllerBase
    {
        private readonly ComixAPIContext _context;

        public GenresController(ComixAPIContext context)
        {
            _context = context;
        }

        public class GenreRequest
        {
            public string Name { get; set; }
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
                return NotFound("Жанр не знайдено.");
            }

            return genre;
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> PutGenre(int id, GenreRequest request)
        {
            var currentGenre = await _context.Genres.FindAsync(id);
            if (currentGenre == null)
            {
                return NotFound("Жанр не знайдено.");
            }

            if (string.IsNullOrWhiteSpace(request.Name))
            {
                return BadRequest("Назва жанру не може бути порожньою.");
            }

            var duplicateGenre = await _context.Genres
                .AnyAsync(g => g.Name.ToLower() == request.Name.ToLower() && g.Id != id);

            if (duplicateGenre)
            {
                return BadRequest("Жанр з такою назвою вже існує.");
            }

            currentGenre.Name = request.Name;
            currentGenre.Modified = DateTime.Now;

            await _context.SaveChangesAsync();

            return NoContent();
        }

        [HttpPost]
        public async Task<ActionResult<Genre>> PostGenre(GenreRequest request)
        {
            if (string.IsNullOrWhiteSpace(request.Name))
            {
                return BadRequest("Назва жанру не може бути порожньою.");
            }

            var duplicateGenre = await _context.Genres
                .AnyAsync(g => g.Name.ToLower() == request.Name.ToLower());

            if (duplicateGenre)
            {
                return BadRequest("Жанр з такою назвою вже існує.");
            }

            var genre = new Genre
            {
                Name = request.Name,
                Created = DateTime.Now,
                Modified = DateTime.Now
            };

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
                return NotFound("Жанр не знайдено.");
            }

            _context.Genres.Remove(genre);
            await _context.SaveChangesAsync();

            return NoContent();
        }
    }
}