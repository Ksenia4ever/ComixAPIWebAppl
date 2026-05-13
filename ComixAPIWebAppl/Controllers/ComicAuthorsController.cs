using ComixAPIWebApp.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace ComixAPIWebApp.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ComicAuthorsController : ControllerBase
    {
        private readonly ComixAPIContext _context;

        public ComicAuthorsController(ComixAPIContext context)
        {
            _context = context;
        }

        public class ComicAuthorRequest
        {
            public int ComicId { get; set; }
            public int AuthorId { get; set; }
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<ComicAuthor>>> GetComicAuthors()
        {
            return await _context.ComicAuthors
                .Include(ca => ca.Comic)
                .Include(ca => ca.Author)
                .ToListAsync();
        }

        [HttpGet("comic/{comicId}")]
        public async Task<ActionResult<IEnumerable<ComicAuthor>>> GetByComic(int comicId)
        {
            return await _context.ComicAuthors
                .Where(ca => ca.ComicId == comicId)
                .Include(ca => ca.Author)
                .ToListAsync();
        }

        [HttpGet("author/{authorId}")]
        public async Task<ActionResult<IEnumerable<ComicAuthor>>> GetByAuthor(int authorId)
        {
            return await _context.ComicAuthors
                .Where(ca => ca.AuthorId == authorId)
                .Include(ca => ca.Comic)
                .ToListAsync();
        }

        [HttpPost]
        public async Task<ActionResult<ComicAuthor>> PostComicAuthor(ComicAuthorRequest request)
        {
            var comicExists = await _context.Comics.AnyAsync(c => c.Id == request.ComicId);
            if (!comicExists)
            {
                return BadRequest("Комікс з таким Id не існує.");
            }

            var authorExists = await _context.Authors.AnyAsync(a => a.Id == request.AuthorId);
            if (!authorExists)
            {
                return BadRequest("Автор з таким Id не існує.");
            }

            var relationExists = await _context.ComicAuthors
                .AnyAsync(ca => ca.ComicId == request.ComicId && ca.AuthorId == request.AuthorId);

            if (relationExists)
            {
                return BadRequest("Такий зв’язок між коміксом і автором вже існує.");
            }

            var comicAuthor = new ComicAuthor
            {
                ComicId = request.ComicId,
                AuthorId = request.AuthorId
            };

            _context.ComicAuthors.Add(comicAuthor);
            await _context.SaveChangesAsync();

            return CreatedAtAction(nameof(GetComicAuthors),
                new { comicId = comicAuthor.ComicId, authorId = comicAuthor.AuthorId },
                comicAuthor);
        }

        [HttpDelete("{comicId}/{authorId}")]
        public async Task<IActionResult> DeleteComicAuthor(int comicId, int authorId)
        {
            var comicAuthor = await _context.ComicAuthors.FindAsync(comicId, authorId);
            if (comicAuthor == null)
            {
                return NotFound();
            }

            _context.ComicAuthors.Remove(comicAuthor);
            await _context.SaveChangesAsync();

            return NoContent();
        }
    }
}