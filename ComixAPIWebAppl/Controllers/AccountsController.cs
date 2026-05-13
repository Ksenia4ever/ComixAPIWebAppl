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

        public class AccountRequest
        {
            public string Name { get; set; }
            public string Email { get; set; }
            public string Password { get; set; }
            public int RoleId { get; set; }
            public bool IsBlocked { get; set; }
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
        public async Task<IActionResult> PutAccount(int id, AccountRequest request)
        {
            var account = await _context.Accounts.FindAsync(id);
            if (account == null)
            {
                return NotFound();
            }

            var roleExists = await _context.Roles.AnyAsync(r => r.Id == request.RoleId);
            if (!roleExists)
            {
                return BadRequest("Роль з таким Id не існує.");
            }

            var emailExists = await _context.Accounts.AnyAsync(a => a.Email == request.Email && a.Id != id);
            if (emailExists)
            {
                return BadRequest("Користувач з таким email вже існує.");
            }

            account.Name = request.Name;
            account.Email = request.Email;
            account.Password = request.Password;
            account.RoleId = request.RoleId;
            account.IsBlocked = request.IsBlocked;
            account.Modified = DateTime.Now;

            await _context.SaveChangesAsync();

            return NoContent();
        }

        [HttpPost]
        public async Task<ActionResult<Account>> PostAccount(AccountRequest request)
        {
            var roleExists = await _context.Roles.AnyAsync(r => r.Id == request.RoleId);
            if (!roleExists)
            {
                return BadRequest("Роль з таким Id не існує.");
            }

            var emailExists = await _context.Accounts.AnyAsync(a => a.Email == request.Email);
            if (emailExists)
            {
                return BadRequest("Користувач з таким email вже існує.");
            }

            var account = new Account
            {
                Name = request.Name,
                Email = request.Email,
                Password = request.Password,
                RoleId = request.RoleId,
                IsBlocked = request.IsBlocked,
                Created = DateTime.Now,
                Modified = DateTime.Now
            };

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