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

        public class CartRequest
        {
            public int AccountId { get; set; }
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
        public async Task<IActionResult> PutCart(int id, CartRequest request)
        {
            var cart = await _context.Carts.FindAsync(id);
            if (cart == null)
            {
                return NotFound();
            }

            var accountExists = await _context.Accounts.AnyAsync(a => a.Id == request.AccountId);
            if (!accountExists)
            {
                return BadRequest("Користувач з таким Id не існує.");
            }

            var anotherCartExists = await _context.Carts.AnyAsync(c => c.AccountId == request.AccountId && c.Id != id);
            if (anotherCartExists)
            {
                return BadRequest("У цього користувача вже є кошик.");
            }

            cart.AccountId = request.AccountId;
            cart.Modified = DateTime.Now;

            await _context.SaveChangesAsync();

            return NoContent();
        }

        [HttpPost]
        public async Task<ActionResult<Cart>> PostCart(CartRequest request)
        {
            var accountExists = await _context.Accounts.AnyAsync(a => a.Id == request.AccountId);
            if (!accountExists)
            {
                return BadRequest("Користувач з таким Id не існує.");
            }

            var cartExists = await _context.Carts.AnyAsync(c => c.AccountId == request.AccountId);
            if (cartExists)
            {
                return BadRequest("У цього користувача вже є кошик.");
            }

            var cart = new Cart
            {
                AccountId = request.AccountId,
                Created = DateTime.Now,
                Modified = DateTime.Now
            };

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