using Microsoft.AspNetCore.Mvc;
using Nhom11.Data;

namespace Nhom11.Controllers
{
    public class AccountsController : Controller
    {
        private WebQuanAoContext _context;
        public AccountsController(WebQuanAoContext context)
        {
            _context = context;
        }
        public IActionResult Index()
        {
            var accounts = _context.Accounts.ToList();
            return View(accounts);
        }
        public IActionResult Login(string Username,string Password)
        {
            var account=_context.Accounts.FirstOrDefault(x => x.Username == Username && x.Password == Password);
            if (account != null)
            {
                CookieOptions options = new CookieOptions();
                options.Expires = DateTime.Now.AddDays(7);
                HttpContext.Response.Cookies.Append("username", Username,options);
                
            }
            return RedirectToAction("Login", "Home");
        }
    }
}
