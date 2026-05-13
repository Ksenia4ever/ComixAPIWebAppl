using ComixAPIWebApp.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace ComixAPIWebApp.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class PublishersController : ControllerBase
    {
        private readonly ComixAPIContext _context;

        public PublishersController(ComixAPIContext context)
        {
            _context = context;
        }

        public class PublisherRequest
        {
            public string Name { get; set; }
            public string? Country { get; set; }
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<Publisher>>> GetPublishers()
        {
            return await _context.Publishers.ToListAsync();
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<Publisher>> GetPublisher(int id)
        {
            var publisher = await _context.Publishers.FindAsync(id);

            if (publisher == null)
            {
                return NotFound("Видавництво не знайдено.");
            }

            return publisher;
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> PutPublisher(int id, PublisherRequest request)
        {
            var currentPublisher = await _context.Publishers.FindAsync(id);
            if (currentPublisher == null)
            {
                return NotFound("Видавництво не знайдено.");
            }

            if (string.IsNullOrWhiteSpace(request.Name))
            {
                return BadRequest("Назва видавництва не може бути порожньою.");
            }

            currentPublisher.Name = request.Name;
            currentPublisher.Country = request.Country;
            currentPublisher.Modified = DateTime.Now;

            await _context.SaveChangesAsync();

            return NoContent();
        }

        [HttpPost]
        public async Task<ActionResult<Publisher>> PostPublisher(PublisherRequest request)
        {
            if (string.IsNullOrWhiteSpace(request.Name))
            {
                return BadRequest("Назва видавництва не може бути порожньою.");
            }

            var publisher = new Publisher
            {
                Name = request.Name,
                Country = request.Country,
                Created = DateTime.Now,
                Modified = DateTime.Now
            };

            _context.Publishers.Add(publisher);
            await _context.SaveChangesAsync();

            return CreatedAtAction(nameof(GetPublisher), new { id = publisher.Id }, publisher);
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeletePublisher(int id)
        {
            var publisher = await _context.Publishers.FindAsync(id);
            if (publisher == null)
            {
                return NotFound("Видавництво не знайдено.");
            }

            _context.Publishers.Remove(publisher);
            await _context.SaveChangesAsync();

            return NoContent();
        }
    }
}