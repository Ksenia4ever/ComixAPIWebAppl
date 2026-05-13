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
                return NotFound();
            }

            return publisher;
        }

        [HttpGet("{id}/comics")]
        public async Task<ActionResult<IEnumerable<Comic>>> GetPublisherComics(int id)
        {
            if (!PublisherExists(id))
            {
                return NotFound();
            }

            return await _context.Comics
                .Where(c => c.PublisherId == id)
                .ToListAsync();
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> PutPublisher(int id, Publisher publisher)
        {
            if (id != publisher.Id)
            {
                return BadRequest();
            }

            _context.Entry(publisher).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!PublisherExists(id))
                {
                    return NotFound();
                }

                throw;
            }

            return NoContent();
        }

        [HttpPost]
        public async Task<ActionResult<Publisher>> PostPublisher(Publisher publisher)
        {
            publisher.Created = DateTime.Now;
            publisher.Modified = DateTime.Now;

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
                return NotFound();
            }

            _context.Publishers.Remove(publisher);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        private bool PublisherExists(int id)
        {
            return _context.Publishers.Any(e => e.Id == id);
        }
    }
}


//using System;
//using System.Collections.Generic;
//using System.Linq;
//using System.Threading.Tasks;
//using Microsoft.AspNetCore.Http;
//using Microsoft.AspNetCore.Mvc;
//using Microsoft.EntityFrameworkCore;
//using ComixAPIWebApp.Models;

//namespace ComixAPIWebAppl.Controllers
//{
//    [Route("api/[controller]")]
//    [ApiController]
//    public class PublishersController : ControllerBase
//    {
//        private readonly ComixAPIContext _context;

//        public PublishersController(ComixAPIContext context)
//        {
//            _context = context;
//        }

//        // GET: api/Publishers
//        [HttpGet]
//        public async Task<ActionResult<IEnumerable<Publisher>>> GetPublishers()
//        {
//            return await _context.Publishers.ToListAsync();
//        }

//        // GET: api/Publishers/5
//        [HttpGet("{id}")]
//        public async Task<ActionResult<Publisher>> GetPublisher(int id)
//        {
//            var publisher = await _context.Publishers.FindAsync(id);

//            if (publisher == null)
//            {
//                return NotFound();
//            }

//            return publisher;
//        }

//        // PUT: api/Publishers/5
//        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
//        [HttpPut("{id}")]
//        public async Task<IActionResult> PutPublisher(int id, Publisher publisher)
//        {
//            if (id != publisher.Id)
//            {
//                return BadRequest();
//            }

//            _context.Entry(publisher).State = EntityState.Modified;

//            try
//            {
//                await _context.SaveChangesAsync();
//            }
//            catch (DbUpdateConcurrencyException)
//            {
//                if (!PublisherExists(id))
//                {
//                    return NotFound();
//                }
//                else
//                {
//                    throw;
//                }
//            }

//            return NoContent();
//        }

//        // POST: api/Publishers
//        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
//        [HttpPost]
//        public async Task<ActionResult<Publisher>> PostPublisher(Publisher publisher)
//        {
//            _context.Publishers.Add(publisher);
//            await _context.SaveChangesAsync();

//            return CreatedAtAction("GetPublisher", new { id = publisher.Id }, publisher);
//        }

//        // DELETE: api/Publishers/5
//        [HttpDelete("{id}")]
//        public async Task<IActionResult> DeletePublisher(int id)
//        {
//            var publisher = await _context.Publishers.FindAsync(id);
//            if (publisher == null)
//            {
//                return NotFound();
//            }

//            _context.Publishers.Remove(publisher);
//            await _context.SaveChangesAsync();

//            return NoContent();
//        }

//        private bool PublisherExists(int id)
//        {
//            return _context.Publishers.Any(e => e.Id == id);
//        }
//    }
//}
