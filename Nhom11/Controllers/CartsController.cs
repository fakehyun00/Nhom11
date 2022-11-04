using Microsoft.AspNetCore.Mvc;

namespace Nhom11.Controllers
{
    public class CartsController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
