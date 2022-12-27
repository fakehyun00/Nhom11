using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Nhom11.Data;
using Nhom11.Models;

namespace Nhom11.Areas.Admin.Controllers
{
    [Area("Admin")]
    public class AccountsController : Controller
    {
        private WebQuanAoContext _context;

        public AccountsController(WebQuanAoContext context)
        {
            _context = context;
        }

         public IActionResult Index()
        {
            var accounts=_context.Accounts.ToList();
            return View(accounts);
        }
        public IActionResult Register()
        {
            return View();
        }
        public IActionResult Login()
        {
            return View();
        }
        [HttpPost]
        [ValidateAntiForgeryToken]

        public IActionResult Login(Account account)
        {
            string username = "adminnguyen";
            string password = "adminnguyen";
            var check = _context.Accounts.Where(x=>x.Username==username&&x.Password==password).FirstOrDefault();
           
            if (check==null)
            {
                account.LoginErrorMessage = "Error Username or Password! Try again please!";
                return View("Login", account);

            }
            if (check.IsAdmin == false)
            {
                account.LoginErrorMessage = "Account Does Not Exists! Try again please!";
                return View("Login", account);

            }
            else
            {
                CookieOptions options = new CookieOptions();
                options.Expires = DateTime.Now.AddDays(7);
                HttpContext.Response.Cookies.Append("username", account.Username, options);
                //SessionOptions session=new SessionOptions();
                HttpContext.Session.SetString("User", account.Username);
            }
            return RedirectToAction("Index", "Products");
        }
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null || _context.Accounts == null)
            {
                return NotFound();
            }
            var account = await _context.Accounts.FirstOrDefaultAsync(m => m.Id == id);
            if (account == null)
            {
                return NotFound();
            }
            _context.Accounts.Remove(account);
            await _context.SaveChangesAsync();
            return RedirectToAction("Index");
        }
        public IActionResult LogOut()
        {
            HttpContext.Session.Clear();
            return RedirectToAction("Login", "Accounts");
        }
        public IActionResult Create()
        {
            return View();
        }
        [HttpPost]
        public IActionResult Create([Bind("Username,Password,Email,Phone,Address,FullName,IsAdmin,Status")] Account account)
        {
            if (ModelState.IsValid)
            {
                _context.Accounts.Add(account);
                _context.SaveChanges();

            }
            return RedirectToAction(nameof(Index));
        }

        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null || _context.Accounts == null)
            {
                return NotFound();
            }
            var account = await _context.Accounts.FindAsync(id);
            if (account == null)
            {
                return NotFound();
            }
            return View();
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int? id, [Bind("Username,Password,Email,Phone,Address,FullName,IsAdmin,Status")] Account account)
        {
            if (id != account.Id)
            {
                return NotFound();
            }
            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(account);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!AccountExists(account.Id))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
            }
            else
            {
                return NotFound();
            }
            return RedirectToAction(nameof(Index));
        }
        private bool AccountExists(int id)
        {
            return _context.Accounts.Any(p => p.Id == id);
        }

    }
}
