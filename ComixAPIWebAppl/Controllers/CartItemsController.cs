using ComixAPIWebApp.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace ComixAPIWebApp.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CartItemsController : ControllerBase
    {
        private readonly ComixAPIContext _context;

        public CartItemsController(ComixAPIContext context)
        {
            _context = context;
        }

        public class CartItemRequest
        {
            public int CartId { get; set; }
            public int ComicId { get; set; }
            public int Quantity { get; set; }
            public decimal UnitPrice { get; set; }
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<CartItem>>> GetCartItems()
        {
            return await _context.CartItems
                .Include(ci => ci.Cart)
                .Include(ci => ci.Comic)
                .ToListAsync();
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<CartItem>> GetCartItem(int id)
        {
            var cartItem = await _context.CartItems
                .Include(ci => ci.Cart)
                .Include(ci => ci.Comic)
                .FirstOrDefaultAsync(ci => ci.Id == id);

            if (cartItem == null)
            {
                return NotFound();
            }

            return cartItem;
        }

        [HttpGet("cart/{cartId}")]
        public async Task<ActionResult<IEnumerable<CartItem>>> GetItemsByCart(int cartId)
        {
            return await _context.CartItems
                .Where(ci => ci.CartId == cartId)
                .Include(ci => ci.Comic)
                .ToListAsync();
        }

        [HttpPatch("{id}/quantity")]
        public async Task<IActionResult> ChangeQuantity(int id, [FromBody] int quantity)
        {
            var cartItem = await _context.CartItems.FindAsync(id);
            if (cartItem == null)
            {
                return NotFound();
            }

            if (quantity <= 0)
            {
                return BadRequest("Кількість повинна бути більшою за 0.");
            }

            cartItem.Quantity = quantity;
            await _context.SaveChangesAsync();

            return NoContent();
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> PutCartItem(int id, CartItemRequest request)
        {
            var cartItem = await _context.CartItems.FindAsync(id);
            if (cartItem == null)
            {
                return NotFound();
            }

            var cartExists = await _context.Carts.AnyAsync(c => c.Id == request.CartId);
            if (!cartExists)
            {
                return BadRequest("Кошик з таким Id не існує.");
            }

            var comicExists = await _context.Comics.AnyAsync(c => c.Id == request.ComicId);
            if (!comicExists)
            {
                return BadRequest("Комікс з таким Id не існує.");
            }

            if (request.Quantity <= 0)
            {
                return BadRequest("Кількість повинна бути більшою за 0.");
            }

            var duplicateExists = await _context.CartItems
                .AnyAsync(ci => ci.CartId == request.CartId &&
                                ci.ComicId == request.ComicId &&
                                ci.Id != id);

            if (duplicateExists)
            {
                return BadRequest("Такий комікс уже є в цьому кошику.");
            }

            cartItem.CartId = request.CartId;
            cartItem.ComicId = request.ComicId;
            cartItem.Quantity = request.Quantity;
            cartItem.UnitPrice = request.UnitPrice;

            await _context.SaveChangesAsync();

            return NoContent();
        }

        [HttpPost]
        public async Task<ActionResult<CartItem>> PostCartItem(CartItemRequest request)
        {
            var cartExists = await _context.Carts.AnyAsync(c => c.Id == request.CartId);
            if (!cartExists)
            {
                return BadRequest("Кошик з таким Id не існує.");
            }

            var comicExists = await _context.Comics.AnyAsync(c => c.Id == request.ComicId);
            if (!comicExists)
            {
                return BadRequest("Комікс з таким Id не існує.");
            }

            if (request.Quantity <= 0)
            {
                return BadRequest("Кількість повинна бути більшою за 0.");
            }

            var duplicateExists = await _context.CartItems
                .AnyAsync(ci => ci.CartId == request.CartId && ci.ComicId == request.ComicId);

            if (duplicateExists)
            {
                return BadRequest("Такий комікс уже є в цьому кошику.");
            }

            var cartItem = new CartItem
            {
                CartId = request.CartId,
                ComicId = request.ComicId,
                Quantity = request.Quantity,
                UnitPrice = request.UnitPrice
            };

            _context.CartItems.Add(cartItem);
            await _context.SaveChangesAsync();

            return CreatedAtAction(nameof(GetCartItem), new { id = cartItem.Id }, cartItem);
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteCartItem(int id)
        {
            var cartItem = await _context.CartItems.FindAsync(id);
            if (cartItem == null)
            {
                return NotFound();
            }

            _context.CartItems.Remove(cartItem);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        private bool CartItemExists(int id)
        {
            return _context.CartItems.Any(e => e.Id == id);
        }
    }
}