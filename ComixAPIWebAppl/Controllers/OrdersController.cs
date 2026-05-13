using ComixAPIWebApp.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace ComixAPIWebApp.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class OrdersController : ControllerBase
    {
        private readonly ComixAPIContext _context;

        public OrdersController(ComixAPIContext context)
        {
            _context = context;
        }

        public class OrderRequest
        {
            public int AccountId { get; set; }
            public string Status { get; set; }
            public decimal TotalAmount { get; set; }
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<Order>>> GetOrders()
        {
            return await _context.Orders
                .Include(o => o.Account)
                .Include(o => o.OrderItems)
                .ThenInclude(oi => oi.Comic)
                .ToListAsync();
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<Order>> GetOrder(int id)
        {
            var order = await _context.Orders
                .Include(o => o.Account)
                .Include(o => o.OrderItems)
                .ThenInclude(oi => oi.Comic)
                .FirstOrDefaultAsync(o => o.Id == id);

            if (order == null)
            {
                return NotFound();
            }

            return order;
        }

        [HttpGet("account/{accountId}")]
        public async Task<ActionResult<IEnumerable<Order>>> GetOrdersByAccount(int accountId)
        {
            return await _context.Orders
                .Where(o => o.AccountId == accountId)
                .Include(o => o.OrderItems)
                .ThenInclude(oi => oi.Comic)
                .ToListAsync();
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> PutOrder(int id, OrderRequest request)
        {
            var order = await _context.Orders.FindAsync(id);
            if (order == null)
            {
                return NotFound();
            }

            var accountExists = await _context.Accounts.AnyAsync(a => a.Id == request.AccountId);
            if (!accountExists)
            {
                return BadRequest("Користувач з таким Id не існує.");
            }

            if (request.TotalAmount < 0)
            {
                return BadRequest("Загальна сума не може бути від'ємною.");
            }

            order.AccountId = request.AccountId;
            order.Status = request.Status;
            order.TotalAmount = request.TotalAmount;
            order.Modified = DateTime.Now;

            await _context.SaveChangesAsync();

            return NoContent();
        }

        [HttpPatch("{id}/status")]
        public async Task<IActionResult> ChangeOrderStatus(int id, [FromBody] string status)
        {
            var order = await _context.Orders.FindAsync(id);
            if (order == null)
            {
                return NotFound();
            }

            if (string.IsNullOrWhiteSpace(status))
            {
                return BadRequest("Статус не може бути порожнім.");
            }

            order.Status = status;
            order.Modified = DateTime.Now;

            await _context.SaveChangesAsync();
            return NoContent();
        }

        [HttpPost]
        public async Task<ActionResult<Order>> PostOrder(OrderRequest request)
        {
            var accountExists = await _context.Accounts.AnyAsync(a => a.Id == request.AccountId);
            if (!accountExists)
            {
                return BadRequest("Користувач з таким Id не існує.");
            }

            if (request.TotalAmount < 0)
            {
                return BadRequest("Загальна сума не може бути від'ємною.");
            }

            var order = new Order
            {
                AccountId = request.AccountId,
                Status = request.Status,
                TotalAmount = request.TotalAmount,
                Created = DateTime.Now,
                Modified = DateTime.Now
            };

            _context.Orders.Add(order);
            await _context.SaveChangesAsync();

            return CreatedAtAction(nameof(GetOrder), new { id = order.Id }, order);
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteOrder(int id)
        {
            var order = await _context.Orders.FindAsync(id);
            if (order == null)
            {
                return NotFound();
            }

            _context.Orders.Remove(order);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        private bool OrderExists(int id)
        {
            return _context.Orders.Any(e => e.Id == id);
        }
    }
}