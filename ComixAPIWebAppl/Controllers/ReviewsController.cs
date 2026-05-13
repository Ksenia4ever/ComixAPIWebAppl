using ComixAPIWebApp.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace ComixAPIWebApp.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ReviewsController : ControllerBase
    {
        private readonly ComixAPIContext _context;

        public ReviewsController(ComixAPIContext context)
        {
            _context = context;
        }

        public class ReviewRequest
        {
            public int ComicId { get; set; }
            public int AccountId { get; set; }
            public int Rating { get; set; }
            public string? Comment { get; set; }
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<Review>>> GetReviews()
        {
            return await _context.Reviews
                .Include(r => r.Account)
                .Include(r => r.Comic)
                .ToListAsync();
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<Review>> GetReview(int id)
        {
            var review = await _context.Reviews
                .Include(r => r.Account)
                .Include(r => r.Comic)
                .FirstOrDefaultAsync(r => r.Id == id);

            if (review == null)
            {
                return NotFound();
            }

            return review;
        }

        [HttpGet("comic/{comicId}")]
        public async Task<ActionResult<IEnumerable<Review>>> GetReviewsByComic(int comicId)
        {
            return await _context.Reviews
                .Where(r => r.ComicId == comicId)
                .Include(r => r.Account)
                .ToListAsync();
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> PutReview(int id, ReviewRequest request)
        {
            var review = await _context.Reviews.FindAsync(id);
            if (review == null)
            {
                return NotFound();
            }

            var comicExists = await _context.Comics.AnyAsync(c => c.Id == request.ComicId);
            if (!comicExists)
            {
                return BadRequest("Комікс з таким Id не існує.");
            }

            var accountExists = await _context.Accounts.AnyAsync(a => a.Id == request.AccountId);
            if (!accountExists)
            {
                return BadRequest("Користувач з таким Id не існує.");
            }

            if (request.Rating < 1 || request.Rating > 5)
            {
                return BadRequest("Оцінка повинна бути від 1 до 5.");
            }

            var duplicateExists = await _context.Reviews
                .AnyAsync(r => r.ComicId == request.ComicId &&
                               r.AccountId == request.AccountId &&
                               r.Id != id);

            if (duplicateExists)
            {
                return BadRequest("Такий відгук уже існує.");
            }

            review.ComicId = request.ComicId;
            review.AccountId = request.AccountId;
            review.Rating = request.Rating;
            review.Comment = request.Comment;
            review.Modified = DateTime.Now;

            await _context.SaveChangesAsync();

            return NoContent();
        }

        [HttpPost]
        public async Task<ActionResult<Review>> PostReview(ReviewRequest request)
        {
            var comicExists = await _context.Comics.AnyAsync(c => c.Id == request.ComicId);
            if (!comicExists)
            {
                return BadRequest("Комікс з таким Id не існує.");
            }

            var accountExists = await _context.Accounts.AnyAsync(a => a.Id == request.AccountId);
            if (!accountExists)
            {
                return BadRequest("Користувач з таким Id не існує.");
            }

            if (request.Rating < 1 || request.Rating > 5)
            {
                return BadRequest("Оцінка повинна бути від 1 до 5.");
            }

            var duplicateExists = await _context.Reviews
                .AnyAsync(r => r.ComicId == request.ComicId && r.AccountId == request.AccountId);

            if (duplicateExists)
            {
                return BadRequest("Такий відгук уже існує.");
            }

            var review = new Review
            {
                ComicId = request.ComicId,
                AccountId = request.AccountId,
                Rating = request.Rating,
                Comment = request.Comment,
                Created = DateTime.Now,
                Modified = DateTime.Now
            };

            _context.Reviews.Add(review);
            await _context.SaveChangesAsync();

            return CreatedAtAction(nameof(GetReview), new { id = review.Id }, review);
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteReview(int id)
        {
            var review = await _context.Reviews.FindAsync(id);
            if (review == null)
            {
                return NotFound();
            }

            _context.Reviews.Remove(review);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        private bool ReviewExists(int id)
        {
            return _context.Reviews.Any(e => e.Id == id);
        }
    }
}