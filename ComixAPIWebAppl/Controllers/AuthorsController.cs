using ComixAPIWebApp.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace ComixAPIWebApp.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthorsController : ControllerBase
    {
        private readonly ComixAPIContext _context;

        public AuthorsController(ComixAPIContext context)
        {
            _context = context;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<Author>>> GetAuthors()
        {
            return await _context.Authors
                .Include(a => a.ComicAuthors)
                .ToListAsync();
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<Author>> GetAuthor(int id)
        {
            var author = await _context.Authors
                .Include(a => a.ComicAuthors)
                .ThenInclude(ca => ca.Comic)
                .FirstOrDefaultAsync(a => a.Id == id);

            if (author == null)
            {
                return NotFound();
            }

            return author;
        }

        [HttpGet("{id}/comics")]
        public async Task<ActionResult<IEnumerable<Comic>>> GetAuthorComics(int id)
        {
            var author = await _context.Authors
                .Include(a => a.ComicAuthors)
                .ThenInclude(ca => ca.Comic)
                .FirstOrDefaultAsync(a => a.Id == id);

            if (author == null)
            {
                return NotFound();
            }

            return author.ComicAuthors.Select(ca => ca.Comic).ToList();
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> PutAuthor(int id, Author author)
        {
            if (id != author.Id)
            {
                return BadRequest();
            }

            _context.Entry(author).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!AuthorExists(id))
                {
                    return NotFound();
                }

                throw;
            }

            return NoContent();
        }

        [HttpPost]
        public async Task<ActionResult<Author>> PostAuthor(Author author)
        {
            author.Created = DateTime.Now;
            author.Modified = DateTime.Now;

            _context.Authors.Add(author);
            await _context.SaveChangesAsync();

            return CreatedAtAction(nameof(GetAuthor), new { id = author.Id }, author);
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteAuthor(int id)
        {
            var author = await _context.Authors.FindAsync(id);
            if (author == null)
            {
                return NotFound();
            }

            _context.Authors.Remove(author);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        private bool AuthorExists(int id)
        {
            return _context.Authors.Any(e => e.Id == id);
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
//    public class AuthorsController : ControllerBase
//    {
//        private readonly ComixAPIContext _context;

//        public AuthorsController(ComixAPIContext context)
//        {
//            _context = context;
//        }

//        // GET: api/Authors
//        [HttpGet]
//        public async Task<ActionResult<IEnumerable<Author>>> GetAuthors()
//        {
//            return await _context.Authors.ToListAsync();
//        }

//        // GET: api/Authors/5
//        [HttpGet("{id}")]
//        public async Task<ActionResult<Author>> GetAuthor(int id)
//        {
//            var author = await _context.Authors.FindAsync(id);

//            if (author == null)
//            {
//                return NotFound();
//            }

//            return author;
//        }

//        // PUT: api/Authors/5
//        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
//        [HttpPut("{id}")]
//        public async Task<IActionResult> PutAuthor(int id, Author author)
//        {
//            if (id != author.Id)
//            {
//                return BadRequest();
//            }

//            _context.Entry(author).State = EntityState.Modified;

//            try
//            {
//                await _context.SaveChangesAsync();
//            }
//            catch (DbUpdateConcurrencyException)
//            {
//                if (!AuthorExists(id))
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

//        // POST: api/Authors
//        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
//        [HttpPost]
//        public async Task<ActionResult<Author>> PostAuthor(Author author)
//        {
//            _context.Authors.Add(author);
//            await _context.SaveChangesAsync();

//            return CreatedAtAction("GetAuthor", new { id = author.Id }, author);
//        }

//        // DELETE: api/Authors/5
//        [HttpDelete("{id}")]
//        public async Task<IActionResult> DeleteAuthor(int id)
//        {
//            var author = await _context.Authors.FindAsync(id);
//            if (author == null)
//            {
//                return NotFound();
//            }

//            _context.Authors.Remove(author);
//            await _context.SaveChangesAsync();

//            return NoContent();
//        }

//        private bool AuthorExists(int id)
//        {
//            return _context.Authors.Any(e => e.Id == id);
//        }
//    }
//}
