using Microsoft.AspNetCore.Mvc;
using Nhom11.Data;

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

        [Area("Admin")]
         public IActionResult Index()
        {
            return View();
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
        public ActionResult Login(string username, string password)
        {
            username = "admin";
            password = "admin";
            var account = _context.Accounts.FirstOrDefault(s => s.Username == username && s.Password == password);
            if (account == null)
            {
                return NotFound();
            }

            return RedirectToAction("Products","Index");
        }
    }
}
