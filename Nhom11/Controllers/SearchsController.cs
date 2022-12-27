using Microsoft.AspNetCore.Mvc;
using Nhom11.Data;

namespace Nhom11.Controllers
{
    public class SearchsController : Controller
    {
        private WebQuanAoContext _context;
        public SearchsController(WebQuanAoContext context)
        {
                _context = context;
        }
        public IActionResult SearchResult(string keyword)
        {
            var products=_context.Products.Where(p=>p.Name.Contains(keyword) && p.Size == 1);
            return View(products);
        }
    }
}
