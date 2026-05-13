using ComixAPIWebApp.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace ComixAPIWebApp.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CartsController : ControllerBase
    {
        private readonly ComixAPIContext _context;

        public CartsController(ComixAPIContext context)
        {
            _context = context;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<Cart>>> GetCarts()
        {
            return await _context.Carts
                .Include(c => c.Account)
                .Include(c => c.CartItems)
                .ThenInclude(ci => ci.Comic)
                .ToListAsync();
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<Cart>> GetCart(int id)
        {
            var cart = await _context.Carts
                .Include(c => c.Account)
                .Include(c => c.CartItems)
                .ThenInclude(ci => ci.Comic)
                .FirstOrDefaultAsync(c => c.Id == id);

            if (cart == null)
            {
                return NotFound();
            }

            return cart;
        }

        [HttpGet("account/{accountId}")]
        public async Task<ActionResult<Cart>> GetCartByAccount(int accountId)
        {
            var cart = await _context.Carts
                .Include(c => c.CartItems)
                .ThenInclude(ci => ci.Comic)
                .FirstOrDefaultAsync(c => c.AccountId == accountId);

            if (cart == null)
            {
                return NotFound();
            }

            return cart;
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> PutCart(int id, Cart cart)
        {
            if (id != cart.Id)
            {
                return BadRequest();
            }

            cart.Modified = DateTime.Now;
            _context.Entry(cart).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!CartExists(id))
                {
                    return NotFound();
                }

                throw;
            }

            return NoContent();
        }

        [HttpPost]
        public async Task<ActionResult<Cart>> PostCart(Cart cart)
        {
            cart.Created = DateTime.Now;
            cart.Modified = DateTime.Now;

            _context.Carts.Add(cart);
            await _context.SaveChangesAsync();

            return CreatedAtAction(nameof(GetCart), new { id = cart.Id }, cart);
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteCart(int id)
        {
            var cart = await _context.Carts.FindAsync(id);
            if (cart == null)
            {
                return NotFound();
            }

            _context.Carts.Remove(cart);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        private bool CartExists(int id)
        {
            return _context.Carts.Any(e => e.Id == id);
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
//    public class CartsController : ControllerBase
//    {
//        private readonly ComixAPIContext _context;

//        public CartsController(ComixAPIContext context)
//        {
//            _context = context;
//        }

//        // GET: api/Carts
//        [HttpGet]
//        public async Task<ActionResult<IEnumerable<Cart>>> GetCarts()
//        {
//            return await _context.Carts.ToListAsync();
//        }

//        // GET: api/Carts/5
//        [HttpGet("{id}")]
//        public async Task<ActionResult<Cart>> GetCart(int id)
//        {
//            var cart = await _context.Carts.FindAsync(id);

//            if (cart == null)
//            {
//                return NotFound();
//            }

//            return cart;
//        }

//        // PUT: api/Carts/5
//        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
//        [HttpPut("{id}")]
//        public async Task<IActionResult> PutCart(int id, Cart cart)
//        {
//            if (id != cart.Id)
//            {
//                return BadRequest();
//            }

//            _context.Entry(cart).State = EntityState.Modified;

//            try
//            {
//                await _context.SaveChangesAsync();
//            }
//            catch (DbUpdateConcurrencyException)
//            {
//                if (!CartExists(id))
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

//        // POST: api/Carts
//        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
//        [HttpPost]
//        public async Task<ActionResult<Cart>> PostCart(Cart cart)
//        {
//            _context.Carts.Add(cart);
//            await _context.SaveChangesAsync();

//            return CreatedAtAction("GetCart", new { id = cart.Id }, cart);
//        }

//        // DELETE: api/Carts/5
//        [HttpDelete("{id}")]
//        public async Task<IActionResult> DeleteCart(int id)
//        {
//            var cart = await _context.Carts.FindAsync(id);
//            if (cart == null)
//            {
//                return NotFound();
//            }

//            _context.Carts.Remove(cart);
//            await _context.SaveChangesAsync();

//            return NoContent();
//        }

//        private bool CartExists(int id)
//        {
//            return _context.Carts.Any(e => e.Id == id);
//        }
//    }
//}
