using ComixAPIWebApp.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace ComixAPIWebApp.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AccountsController : ControllerBase
    {
        private readonly ComixAPIContext _context;

        public AccountsController(ComixAPIContext context)
        {
            _context = context;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<Account>>> GetAccounts()
        {
            return await _context.Accounts
                .Include(a => a.Role)
                .Include(a => a.Cart)
                .ToListAsync();
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<Account>> GetAccount(int id)
        {
            var account = await _context.Accounts
                .Include(a => a.Role)
                .Include(a => a.Cart)
                .Include(a => a.Orders)
                .Include(a => a.Reviews)
                .FirstOrDefaultAsync(a => a.Id == id);

            if (account == null)
            {
                return NotFound();
            }

            return account;
        }

        [HttpGet("{id}/orders")]
        public async Task<ActionResult<IEnumerable<Order>>> GetAccountOrders(int id)
        {
            if (!AccountExists(id))
            {
                return NotFound();
            }

            return await _context.Orders
                .Where(o => o.AccountId == id)
                .Include(o => o.OrderItems)
                .ToListAsync();
        }

        [HttpPatch("{id}/block")]
        public async Task<IActionResult> BlockAccount(int id)
        {
            var account = await _context.Accounts.FindAsync(id);
            if (account == null)
            {
                return NotFound();
            }

            account.IsBlocked = true;
            account.Modified = DateTime.Now;

            await _context.SaveChangesAsync();
            return NoContent();
        }

        [HttpPatch("{id}/unblock")]
        public async Task<IActionResult> UnblockAccount(int id)
        {
            var account = await _context.Accounts.FindAsync(id);
            if (account == null)
            {
                return NotFound();
            }

            account.IsBlocked = false;
            account.Modified = DateTime.Now;

            await _context.SaveChangesAsync();
            return NoContent();
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> PutAccount(int id, Account account)
        {
            if (id != account.Id)
            {
                return BadRequest();
            }

            account.Modified = DateTime.Now;
            _context.Entry(account).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!AccountExists(id))
                {
                    return NotFound();
                }

                throw;
            }

            return NoContent();
        }

        [HttpPost]
        public async Task<ActionResult<Account>> PostAccount(Account account)
        {
            account.Created = DateTime.Now;
            account.Modified = DateTime.Now;

            _context.Accounts.Add(account);
            await _context.SaveChangesAsync();

            return CreatedAtAction(nameof(GetAccount), new { id = account.Id }, account);
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteAccount(int id)
        {
            var account = await _context.Accounts.FindAsync(id);
            if (account == null)
            {
                return NotFound();
            }

            _context.Accounts.Remove(account);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        private bool AccountExists(int id)
        {
            return _context.Accounts.Any(e => e.Id == id);
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
//    public class AccountsController : ControllerBase
//    {
//        private readonly ComixAPIContext _context;

//        public AccountsController(ComixAPIContext context)
//        {
//            _context = context;
//        }

//        // GET: api/Accounts
//        [HttpGet]
//        public async Task<ActionResult<IEnumerable<Account>>> GetAccounts()
//        {
//            return await _context.Accounts.ToListAsync();
//        }

//        // GET: api/Accounts/5
//        [HttpGet("{id}")]
//        public async Task<ActionResult<Account>> GetAccount(int id)
//        {
//            var account = await _context.Accounts.FindAsync(id);

//            if (account == null)
//            {
//                return NotFound();
//            }

//            return account;
//        }

//        // PUT: api/Accounts/5
//        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
//        [HttpPut("{id}")]
//        public async Task<IActionResult> PutAccount(int id, Account account)
//        {
//            if (id != account.Id)
//            {
//                return BadRequest();
//            }

//            _context.Entry(account).State = EntityState.Modified;

//            try
//            {
//                await _context.SaveChangesAsync();
//            }
//            catch (DbUpdateConcurrencyException)
//            {
//                if (!AccountExists(id))
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

//        // POST: api/Accounts
//        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
//        [HttpPost]
//        public async Task<ActionResult<Account>> PostAccount(Account account)
//        {
//            _context.Accounts.Add(account);
//            await _context.SaveChangesAsync();

//            return CreatedAtAction("GetAccount", new { id = account.Id }, account);
//        }

//        // DELETE: api/Accounts/5
//        [HttpDelete("{id}")]
//        public async Task<IActionResult> DeleteAccount(int id)
//        {
//            var account = await _context.Accounts.FindAsync(id);
//            if (account == null)
//            {
//                return NotFound();
//            }

//            _context.Accounts.Remove(account);
//            await _context.SaveChangesAsync();

//            return NoContent();
//        }

//        private bool AccountExists(int id)
//        {
//            return _context.Accounts.Any(e => e.Id == id);
//        }
//    }
//}
