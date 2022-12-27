using Microsoft.AspNetCore.Mvc;
using Nhom11.Data;
using Nhom11.Models;

namespace Nhom11.Areas.Admin.Controllers
{
    [Area("Admin")]
    public class ProductTypesController : Controller
    {
        private WebQuanAoContext _context;
        public  ProductTypesController(WebQuanAoContext context)
        {
                _context = context;
        }
        public IActionResult Index()
        {
            var types = _context.ProductTypes.ToList();
            return View(types);
        }
        public IActionResult Create()
        {
            return View();
        }
        [HttpPost]
        public async Task<IActionResult> Create([Bind("Name,Status")] ProductType producttype)
        {
            if (ModelState.IsValid)
            {
                _context.ProductTypes.Add(producttype);
                await _context.SaveChangesAsync();

            }
            return RedirectToAction(nameof(Index));
        }
    }
}
