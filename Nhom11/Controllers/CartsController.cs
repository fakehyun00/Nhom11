using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Nhom11.Data;
using Nhom11.Models;

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
        public IActionResult Purchase()
        {
            return View();
        }
        [HttpPost]
        public IActionResult Purchase(string ShippingAddress,string ShippingPhone)
        {
            string username = "dung";
            var cart=_context.Carts.Include(c=>c.Account).Include(c=>c.Product).Where(c=>c.Account.Username==username);
            var accountid=_context.Accounts.FirstOrDefault(a=>a.Username==username).Id;
            var total=cart.Sum(c=>c.Product.Price*c.Quantity);
            Invoice invoice = new Invoice()
            {
                Code = DateTime.Now.ToString("yyyyMMddhhmmss"),
                AccountId=accountid,
                IssuedDate=DateTime.Now,
                ShippingAddress=ShippingAddress,
                ShippingPhone=ShippingPhone,
                Total=total,
                Status=true,
            };
            _context.Invoices.Add(invoice);
            _context.SaveChanges();
            foreach(var item in cart)
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
            return RedirectToAction("Index","Products");
        }
        [HttpPost]
        public async Task<IActionResult> Add(int ProductId,int Quantity,int Size)
        {
            string username = "dung";
            var accountid = _context.Accounts.FirstOrDefault(a => a.Username == username).Id;
            var productsize=_context.Products.Where(p=>p.Size == Size);
            var carts = _context.Carts.Where(c => c.AccountId == accountid && c.ProductId == ProductId);
            if (carts!= null)
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
            }
            _context.SaveChanges();
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
