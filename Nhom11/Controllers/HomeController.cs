using Microsoft.AspNetCore.Mvc;
using Nhom11.Data;
using Nhom11.Models;
using System.Diagnostics;
using System.Security.Cryptography;
using System.Text;
using Microsoft.AspNetCore.Session;


namespace Nhom11.Controllers
{
    public class HomeController : Controller
    {
        private WebQuanAoContext _context;
        private readonly ILogger<HomeController> _logger;
       

        public HomeController(ILogger<HomeController> logger, WebQuanAoContext context)
        {
            _logger = logger;
            _context = context;
        }

        public IActionResult Index()
        {
            var products = _context.Products.Where(p => p.Price == 145000 && p.Size==1);
            return View(products);
        }

        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
        public IActionResult Detail(int? id)
        {
            var product = _context.Products.FirstOrDefault(x => x.Id == id);
            if (product == null)
            {
                return NotFound();
            }
            if (id == null)
            {
                return RedirectToAction("Index");
            }
            return View(product);
        }

       
        


        //Logout
        
    }
}