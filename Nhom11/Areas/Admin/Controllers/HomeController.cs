using Microsoft.AspNetCore.Mvc;
using Nhom11.Data;
using Nhom11.Models;

namespace Nhom11.Areas.Admin.Controllers
{
    [Area("Admin")]
    public class HomeController : Controller
    {
        private WebQuanAoContext _context;
        private IWebHostEnvironment _enviroment;
        public HomeController(WebQuanAoContext context,IWebHostEnvironment environment)
        {
            _context = context;
            _enviroment = environment;
        }
        public IActionResult Index()
        {
            var products = _context.Products.ToList();
            return View(products);
        }
        public IActionResult Login()
        {
            return View();
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Login(string Username, string Password)
        {
            var account=_context.Accounts.FirstOrDefault(x => x.Username == Username&&x.Password==Password);
            if(account!=null)
            {
                var id=account.Id;
                var fullname=account.FullName;
                var checklogin = 1;
                HttpContext.Session.SetInt32("checklogin", checklogin);
                HttpContext.Session.SetInt32("id", id);
                HttpContext.Session.SetString("fullname", fullname);
                HttpContext.Session.SetString("username", Username);
                if (account.IsAdmin == true)
                {
                    return RedirectToAction("Home", "GetProducts");
                }
                else
                {
                    return RedirectToAction("Products", "GetProducts");
                }
            }
            else
            {
                ViewBag.ErrorMsg = "Login Failed";
                return View();
            }
            
        }
        public IActionResult Logout()
        {
            HttpContext.Session.Clear();
            return RedirectToAction("Index", "Home");
        }
        public IActionResult Register()
        {
            return View();
        }
        public async Task<IActionResult> Delete(int? id)
        {
            var product = await _context.Products.FindAsync(id);
            if (product != null)
            {
                _context.Products.Remove(product);
            }
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }
        public IActionResult Create()
        {
            return View();
        }
        [HttpPost]
        public async Task<IActionResult> Create([Bind("Image,ImageFile,Name,Size,Price,ProductTypeId,Stock,Status")] Product product)
        {
            if (ModelState.IsValid)
            {
                _context.Products.Add(product);
                await _context.SaveChangesAsync();

                if (product.ImageFile != null)
                {
                    var fileName = product.Id.ToString() + Path.GetExtension(product.ImageFile.FileName);
                    var uploadFolfder = Path.Combine(_enviroment.WebRootPath, "images");
                    var uploadPath = Path.Combine(uploadFolfder, fileName);
                    using (FileStream fs = System.IO.File.Create(uploadPath))
                    {
                        product.ImageFile.CopyTo(fs);
                        fs.Flush();
                    }
                    product.Image = fileName;
                    _context.Products.Add(product);
                    _context.SaveChanges();
                }

            }
            return RedirectToAction(nameof(Index));
        }
    }
}
