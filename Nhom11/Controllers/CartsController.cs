using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Nhom11.Data;

namespace Nhom11.Controllers
{
    public class CartsController : Controller
    {
        private WebQuanAoContext _context;
        public CartsController(WebQuanAoContext context)
        {
            _context = context;
        }
        public async Task<IActionResult> Index()
        {
            string username = "dung";
            var carts=_context.Carts.Include(c=>c.Account).Include(c=>c.Product).Where(c=>c.Account.Username == username);
            return View(await carts.ToListAsync());
        }
        
    }
}
