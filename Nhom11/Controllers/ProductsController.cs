using Microsoft.AspNetCore.Mvc;
using Nhom11.Data;

namespace Nhom11.Controllers
{
    public class ProductsController : Controller
    {
        private WebQuanAoContext _context;
        public ProductsController(WebQuanAoContext context)
        {
            _context = context;
        }
        public IActionResult Index()
        {
            var products = _context.Products.ToList();
            return View(products);
        }
        public IActionResult Detail(int? id)
		{
            var product = _context.Products.FirstOrDefault(x => x.Id == id);
            if(product == null)
			{
                return NotFound();
			}
            if(id==null)
			{
                return RedirectToAction("Index");
			}
            return View(product);
		}
        public IActionResult Login()
        {
            return View();
        }
    }
}
