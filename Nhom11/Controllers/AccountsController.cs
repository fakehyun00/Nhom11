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
        public IActionResult Login()
        {
            return View();
        }
    }
}
