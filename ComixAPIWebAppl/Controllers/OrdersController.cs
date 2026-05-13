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
        public async Task<IActionResult> PutOrder(int id, Order order)
        {
            if (id != order.Id)
            {
                return BadRequest();
            }

            order.Modified = DateTime.Now;
            _context.Entry(order).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!OrderExists(id))
                {
                    return NotFound();
                }

                throw;
            }

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

            order.Status = status;
            order.Modified = DateTime.Now;

            await _context.SaveChangesAsync();
            return NoContent();
        }

        [HttpPost]
        public async Task<ActionResult<Order>> PostOrder(Order order)
        {
            order.Created = DateTime.Now;
            order.Modified = DateTime.Now;

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
//    public class OrdersController : ControllerBase
//    {
//        private readonly ComixAPIContext _context;

//        public OrdersController(ComixAPIContext context)
//        {
//            _context = context;
//        }

//        // GET: api/Orders
//        [HttpGet]
//        public async Task<ActionResult<IEnumerable<Order>>> GetOrders()
//        {
//            return await _context.Orders.ToListAsync();
//        }

//        // GET: api/Orders/5
//        [HttpGet("{id}")]
//        public async Task<ActionResult<Order>> GetOrder(int id)
//        {
//            var order = await _context.Orders.FindAsync(id);

//            if (order == null)
//            {
//                return NotFound();
//            }

//            return order;
//        }

//        // PUT: api/Orders/5
//        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
//        [HttpPut("{id}")]
//        public async Task<IActionResult> PutOrder(int id, Order order)
//        {
//            if (id != order.Id)
//            {
//                return BadRequest();
//            }

//            _context.Entry(order).State = EntityState.Modified;

//            try
//            {
//                await _context.SaveChangesAsync();
//            }
//            catch (DbUpdateConcurrencyException)
//            {
//                if (!OrderExists(id))
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

//        // POST: api/Orders
//        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
//        [HttpPost]
//        public async Task<ActionResult<Order>> PostOrder(Order order)
//        {
//            _context.Orders.Add(order);
//            await _context.SaveChangesAsync();

//            return CreatedAtAction("GetOrder", new { id = order.Id }, order);
//        }

//        // DELETE: api/Orders/5
//        [HttpDelete("{id}")]
//        public async Task<IActionResult> DeleteOrder(int id)
//        {
//            var order = await _context.Orders.FindAsync(id);
//            if (order == null)
//            {
//                return NotFound();
//            }

//            _context.Orders.Remove(order);
//            await _context.SaveChangesAsync();

//            return NoContent();
//        }

//        private bool OrderExists(int id)
//        {
//            return _context.Orders.Any(e => e.Id == id);
//        }
//    }
//}
