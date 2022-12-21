using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Nhom11.Data;
using Nhom11.Models;


namespace Nhom11.Areas.Admin.Controllers
{
    [Area("Admin")]
    public class ProductsController : Controller
    {
        private WebQuanAoContext _context;
        private IWebHostEnvironment _environment;

        public ProductsController(WebQuanAoContext context, IWebHostEnvironment environment)
        {
            _context = context;
            _environment = environment;
        }


         public IActionResult Index()
        {
            var products = _context.Products.ToList();
            return View(products);
        }
        
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null||_context.Products==null)
            {
                return NotFound();
            }
            var product = await _context.Products.FirstOrDefaultAsync(m=>m.Id==id);
            if(product==null)
            {
                return NotFound();
            }
            _context.Products.Remove(product);
            await _context.SaveChangesAsync();
            return RedirectToAction("Index");
        }
        public IActionResult Create()
        {
            return View();
        }
        [HttpPost]
        public async Task<IActionResult> Create([Bind("Image,ImageFile,ImageFile,SKU,Name,Price,Stock,Status")] Product product)
        {
            if (ModelState.IsValid)
            {
                _context.Products.Add(product);
                await _context.SaveChangesAsync();

                if (product.ImageFile != null)
                {
                    var fileName = product.Id.ToString() + Path.GetExtension(product.ImageFile.FileName);
                    var uploadFolfder = Path.Combine(_environment.WebRootPath, "images");
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
