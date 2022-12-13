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
            var products=_context.Products.Where(p=>p.Size==1 );
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
        public async Task<IActionResult> Create([Bind("Image,ImageFile,ImageFile,SKU,Name,Price,Stock,Status")]Product product)
        {
             if(ModelState.IsValid)
            {
                _context.Products.Add(product);
                await _context.SaveChangesAsync();

                if(product.ImageFile!=null)
                {
                    var fileName = product.Id.ToString() + Path.GetExtension(product.ImageFile.FileName);
                    var uploadFolfder = Path.Combine(_environment.WebRootPath, "images");
                    var uploadPath=Path.Combine(uploadFolfder, fileName);
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
        [HttpPost]
        public async Task<IActionResult> Add(int ProductId, int Quantity, int Size)
        {
            string username = "dung";
            var accountid = _context.Accounts.FirstOrDefault(a => a.Username == username).Id;
            var productsize = _context.Products.Where(p => p.Size == Size);
            var carts = _context.Carts.Where(c => c.AccountId == accountid && c.ProductId == ProductId);
            if (carts != null)
            {
                Quantity += Quantity;
                _context.SaveChanges();
            }
            else
            {
                Cart cart = new Cart
                {
                    AccountId = accountid,
                    Quantity = Quantity,
                    ProductId = ProductId,

                };
                _context.Carts.Add(cart);
                _context.Carts.Update(cart);
            }
            _context.SaveChanges();
            return RedirectToAction("Index", "Products");
        }
    }
}
