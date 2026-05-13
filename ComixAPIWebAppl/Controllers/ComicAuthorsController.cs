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
        public async Task<ActionResult<ComicAuthor>> PostComicAuthor(ComicAuthor comicAuthor)
        {
            _context.ComicAuthors.Add(comicAuthor);
            await _context.SaveChangesAsync();

            return CreatedAtAction(nameof(GetComicAuthors), new { comicId = comicAuthor.ComicId, authorId = comicAuthor.AuthorId }, comicAuthor);
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
//    public class ComicAuthorsController : ControllerBase
//    {
//        private readonly ComixAPIContext _context;

//        public ComicAuthorsController(ComixAPIContext context)
//        {
//            _context = context;
//        }

//        // GET: api/ComicAuthors
//        [HttpGet]
//        public async Task<ActionResult<IEnumerable<ComicAuthor>>> GetComicAuthors()
//        {
//            return await _context.ComicAuthors.ToListAsync();
//        }

//        // GET: api/ComicAuthors/5
//        [HttpGet("{id}")]
//        public async Task<ActionResult<ComicAuthor>> GetComicAuthor(int id)
//        {
//            var comicAuthor = await _context.ComicAuthors.FindAsync(id);

//            if (comicAuthor == null)
//            {
//                return NotFound();
//            }

//            return comicAuthor;
//        }

//        // PUT: api/ComicAuthors/5
//        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
//        [HttpPut("{id}")]
//        public async Task<IActionResult> PutComicAuthor(int id, ComicAuthor comicAuthor)
//        {
//            if (id != comicAuthor.ComicId)
//            {
//                return BadRequest();
//            }

//            _context.Entry(comicAuthor).State = EntityState.Modified;

//            try
//            {
//                await _context.SaveChangesAsync();
//            }
//            catch (DbUpdateConcurrencyException)
//            {
//                if (!ComicAuthorExists(id))
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

//        // POST: api/ComicAuthors
//        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
//        [HttpPost]
//        public async Task<ActionResult<ComicAuthor>> PostComicAuthor(ComicAuthor comicAuthor)
//        {
//            _context.ComicAuthors.Add(comicAuthor);
//            try
//            {
//                await _context.SaveChangesAsync();
//            }
//            catch (DbUpdateException)
//            {
//                if (ComicAuthorExists(comicAuthor.ComicId))
//                {
//                    return Conflict();
//                }
//                else
//                {
//                    throw;
//                }
//            }

//            return CreatedAtAction("GetComicAuthor", new { id = comicAuthor.ComicId }, comicAuthor);
//        }

//        // DELETE: api/ComicAuthors/5
//        [HttpDelete("{id}")]
//        public async Task<IActionResult> DeleteComicAuthor(int id)
//        {
//            var comicAuthor = await _context.ComicAuthors.FindAsync(id);
//            if (comicAuthor == null)
//            {
//                return NotFound();
//            }

//            _context.ComicAuthors.Remove(comicAuthor);
//            await _context.SaveChangesAsync();

//            return NoContent();
//        }

//        private bool ComicAuthorExists(int id)
//        {
//            return _context.ComicAuthors.Any(e => e.ComicId == id);
//        }
//    }
//}
