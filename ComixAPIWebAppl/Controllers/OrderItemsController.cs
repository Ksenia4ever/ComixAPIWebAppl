using ComixAPIWebApp.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace ComixAPIWebApp.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class OrderItemsController : ControllerBase
    {
        private readonly ComixAPIContext _context;

        public OrderItemsController(ComixAPIContext context)
        {
            _context = context;
        }

        public class OrderItemRequest
        {
            public int OrderId { get; set; }
            public int ComicId { get; set; }
            public int Quantity { get; set; }
            public decimal UnitPrice { get; set; }
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<OrderItem>>> GetOrderItems()
        {
            return await _context.OrderItems
                .Include(oi => oi.Order)
                .Include(oi => oi.Comic)
                .ToListAsync();
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<OrderItem>> GetOrderItem(int id)
        {
            var orderItem = await _context.OrderItems
                .Include(oi => oi.Order)
                .Include(oi => oi.Comic)
                .FirstOrDefaultAsync(oi => oi.Id == id);

            if (orderItem == null)
            {
                return NotFound();
            }

            return orderItem;
        }

        [HttpGet("order/{orderId}")]
        public async Task<ActionResult<IEnumerable<OrderItem>>> GetItemsByOrder(int orderId)
        {
            return await _context.OrderItems
                .Where(oi => oi.OrderId == orderId)
                .Include(oi => oi.Comic)
                .ToListAsync();
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> PutOrderItem(int id, OrderItemRequest request)
        {
            var orderItem = await _context.OrderItems.FindAsync(id);
            if (orderItem == null)
            {
                return NotFound();
            }

            var orderExists = await _context.Orders.AnyAsync(o => o.Id == request.OrderId);
            if (!orderExists)
            {
                return BadRequest("Замовлення з таким Id не існує.");
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

            orderItem.OrderId = request.OrderId;
            orderItem.ComicId = request.ComicId;
            orderItem.Quantity = request.Quantity;
            orderItem.UnitPrice = request.UnitPrice;

            await _context.SaveChangesAsync();

            return NoContent();
        }

        [HttpPost]
        public async Task<ActionResult<OrderItem>> PostOrderItem(OrderItemRequest request)
        {
            var orderExists = await _context.Orders.AnyAsync(o => o.Id == request.OrderId);
            if (!orderExists)
            {
                return BadRequest("Замовлення з таким Id не існує.");
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

            var orderItem = new OrderItem
            {
                OrderId = request.OrderId,
                ComicId = request.ComicId,
                Quantity = request.Quantity,
                UnitPrice = request.UnitPrice
            };

            _context.OrderItems.Add(orderItem);
            await _context.SaveChangesAsync();

            return CreatedAtAction(nameof(GetOrderItem), new { id = orderItem.Id }, orderItem);
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteOrderItem(int id)
        {
            var orderItem = await _context.OrderItems.FindAsync(id);
            if (orderItem == null)
            {
                return NotFound();
            }

            _context.OrderItems.Remove(orderItem);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        private bool OrderItemExists(int id)
        {
            return _context.OrderItems.Any(e => e.Id == id);
        }
    }
}