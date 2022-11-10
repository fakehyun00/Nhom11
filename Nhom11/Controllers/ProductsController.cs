using Microsoft.AspNetCore.Mvc;
using Nhom11.Data;
using Nhom11.Models;

namespace Nhom11.Controllers
{
    public class ProductsController : Controller
    {
        private WebQuanAoContext _context;
        private IWebHostEnvironment _environment;
        public ProductsController(WebQuanAoContext context,IWebHostEnvironment environment)
        {
            _context = context;
            _environment = environment;
        }
        public IActionResult Index()
        {
            var products=_context.Products.ToList();
            
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
        public IActionResult Create()
        {
            return View();
        }
        [HttpPost]
        public IActionResult Create([Bind("Image,ImageFile,Name,Price,Stock,Status")]Product product)
        {
            
            _context.Products.Add(product);
            _context.SaveChanges();
            if (product == null)
            {
               return NotFound();
            }
            return View();      
        }
    }
}
