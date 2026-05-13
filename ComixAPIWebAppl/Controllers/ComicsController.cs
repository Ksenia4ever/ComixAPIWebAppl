using ComixAPIWebApp.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace ComixAPIWebApp.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ComicsController : ControllerBase
    {
        private readonly ComixAPIContext _context;

        public ComicsController(ComixAPIContext context)
        {
            _context = context;
        }

        public class ComicRequest
        {
            public string Title { get; set; }
            public string Description { get; set; }
            public decimal Price { get; set; }
            public int StockQuantity { get; set; }
            public int? ReleaseYear { get; set; }
            public int PublisherId { get; set; }
            public int GenreId { get; set; }
            public string? CoverImagePath { get; set; }
            public bool IsActive { get; set; }
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<Comic>>> GetComics()
        {
            return await _context.Comics
                .Include(c => c.Genre)
                .Include(c => c.Publisher)
                .Include(c => c.ComicAuthors)
                    .ThenInclude(ca => ca.Author)
                .ToListAsync();
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<Comic>> GetComic(int id)
        {
            var comic = await _context.Comics
                .Include(c => c.Genre)
                .Include(c => c.Publisher)
                .Include(c => c.ComicAuthors)
                    .ThenInclude(ca => ca.Author)
                .FirstOrDefaultAsync(c => c.Id == id);

            if (comic == null)
            {
                return NotFound();
            }

            return comic;
        }

        [HttpGet("genre/{genreId}")]
        public async Task<ActionResult<IEnumerable<Comic>>> GetComicsByGenre(int genreId)
        {
            return await _context.Comics
                .Where(c => c.GenreId == genreId)
                .Include(c => c.Genre)
                .Include(c => c.Publisher)
                .ToListAsync();
        }

        [HttpGet("publisher/{publisherId}")]
        public async Task<ActionResult<IEnumerable<Comic>>> GetComicsByPublisher(int publisherId)
        {
            return await _context.Comics
                .Where(c => c.PublisherId == publisherId)
                .Include(c => c.Genre)
                .Include(c => c.Publisher)
                .ToListAsync();
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> PutComic(int id, ComicRequest request)
        {
            var comic = await _context.Comics.FindAsync(id);
            if (comic == null)
            {
                return NotFound();
            }

            var genreExists = await _context.Genres.AnyAsync(g => g.Id == request.GenreId);
            if (!genreExists)
            {
                return BadRequest("Жанр з таким Id не існує.");
            }

            var publisherExists = await _context.Publishers.AnyAsync(p => p.Id == request.PublisherId);
            if (!publisherExists)
            {
                return BadRequest("Видавництво з таким Id не існує.");
            }

            comic.Title = request.Title;
            comic.Description = request.Description;
            comic.Price = request.Price;
            comic.StockQuantity = request.StockQuantity;
            comic.ReleaseYear = request.ReleaseYear;
            comic.PublisherId = request.PublisherId;
            comic.GenreId = request.GenreId;
            comic.CoverImagePath = request.CoverImagePath;
            comic.IsActive = request.IsActive;
            comic.Modified = DateTime.Now;

            await _context.SaveChangesAsync();

            return NoContent();
        }

        [HttpPost]
        public async Task<ActionResult<Comic>> PostComic(ComicRequest request)
        {
            var genreExists = await _context.Genres.AnyAsync(g => g.Id == request.GenreId);
            if (!genreExists)
            {
                return BadRequest("Жанр з таким Id не існує.");
            }

            var publisherExists = await _context.Publishers.AnyAsync(p => p.Id == request.PublisherId);
            if (!publisherExists)
            {
                return BadRequest("Видавництво з таким Id не існує.");
            }

            var comic = new Comic
            {
                Title = request.Title,
                Description = request.Description,
                Price = request.Price,
                StockQuantity = request.StockQuantity,
                ReleaseYear = request.ReleaseYear,
                PublisherId = request.PublisherId,
                GenreId = request.GenreId,
                CoverImagePath = request.CoverImagePath,
                IsActive = request.IsActive,
                Created = DateTime.Now,
                Modified = DateTime.Now
            };

            _context.Comics.Add(comic);
            await _context.SaveChangesAsync();

            return CreatedAtAction(nameof(GetComic), new { id = comic.Id }, comic);
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteComic(int id)
        {
            var comic = await _context.Comics.FindAsync(id);
            if (comic == null)
            {
                return NotFound();
            }

            _context.Comics.Remove(comic);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        private bool ComicExists(int id)
        {
            return _context.Comics.Any(e => e.Id == id);
        }
    }
}