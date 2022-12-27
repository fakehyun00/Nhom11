using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Nhom11.Data;
using Nhom11.Models;
using Nhom11.Helpers;

namespace Nhom11.Controllers
{
    public class CartsController : Controller
    {
        private WebQuanAoContext _context;
        public CartsController(WebQuanAoContext context)
        {
            _context = context;
        }

        public List<Cart> Carts
        {
            get
            {
                var data = HttpContext.Session.Get<List<Cart>>("Cart");
                if(data == null)
                {
                    data = new List<Cart>();
                }
                return data;
            }
        }


        public async Task<object> Index()
        {
            string username = "vandung";
            
            var carts=_context.Carts.Include(c=>c.Account).Include(c=>c.Product).Where(c=>c.Account.Username == username);
            return View(await carts.ToListAsync());
        }
        public IActionResult Purchase()
        {
            return View();
        }
        [HttpPost]
        public IActionResult Purchase(string ShippingAddress,string ShippingPhone)
        {
            string username = "vandung";
            var cart=_context.Carts.Include(c=>c.Account).Include(c=>c.Product).Where(c=>c.Account.Username==username);
            var accountid=_context.Accounts.FirstOrDefault(a=>a.Username==username).Id;
            var total=cart.Sum(c=>c.Product.Price*c.Quantity);
            if (cart != null)
            {
                Invoice invoice = new Invoice()
                {
                    Code = DateTime.Now.ToString("yyyyMMddhhmmss"),
                    AccountId = accountid,
                    IssuedDate = DateTime.Now,
                    ShippingAddress = ShippingAddress,
                    ShippingPhone = ShippingPhone,
                    Total = total,
                    Status = true,
                };
                _context.Invoices.Add(invoice);
                _context.SaveChanges();
                foreach (var item in cart)
                {
                    InvoiceDetail detail = new InvoiceDetail()
                    {
                        InvoiceId = invoice.Id,
                        ProductId = item.ProductId,
                        Quantity = item.Quantity,
                        UnitPrice = item.Product.Price
                    };
                    _context.InvoiceDetails.Add(detail);
                    _context.Carts.Remove(item);
                    item.Product.Stock -= item.Quantity;
                    _context.Products.Update(item.Product);
                }
                _context.SaveChanges();
                return RedirectToAction("Index", "Products");
            }
            else
            {
                return RedirectToAction("Index");
            }
            
        }
        [HttpPost]
        public async Task<IActionResult> Add(int ProductId,int Quantity,int Size)
        {
            string username = "vandung";
            var accountid = _context.Accounts.FirstOrDefault(a => a.Username == username).Id;
            var productsize=_context.Products.Where(p=>p.Size == Size);
            var carts = _context.Carts.Where(c => c.AccountId == accountid && c.ProductId == ProductId);
            if (carts== null)
            {
                Cart cart = new Cart
                {
                    AccountId = accountid,
                    Quantity = Quantity,
                    ProductId = ProductId,
                };
               await _context.Carts.AddAsync(cart);
            }
            
            else
            {
               
                Quantity += Quantity;
                await _context.SaveChangesAsync();
            }
            await _context.SaveChangesAsync();
            return RedirectToAction("Index","Products");
        }
        public async Task<IActionResult> Delete(int ?id)
        {
            var cart = await _context.Carts.FindAsync(id);
            if(cart!=null)
            {
                _context.Carts.Remove(cart);
            }
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }
        private bool CartExists(int id)
        {
            return _context.Carts.Any(c => c.Id == id);
        }
        
    }
}
