using Microsoft.EntityFrameworkCore;
using Nhom11.Models;

namespace Nhom11.Data
{
    public class WebQuanAoContext:DbContext
    {
        public WebQuanAoContext(DbContextOptions<WebQuanAoContext>options): base(options) { }
        public DbSet<Account> Accounts { get; set; }
        public DbSet<Cart> Carts { get; set; }
        public DbSet<Invoice> Invoices { get; set; }
        public DbSet<InvoiceDetail> InvoiceDetails { get; set; }
        public DbSet<Product> Products { get; set; }
        public DbSet<ProductType> ProductTypes { get; set; }
        public object Configuration { get; internal set; }
    }
}
