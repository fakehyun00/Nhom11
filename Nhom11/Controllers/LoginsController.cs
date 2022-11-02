using Microsoft.AspNetCore.Mvc;
namespace Nhom11.Controllers
{
    public class LoginsController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
        public IActionResult Login()
        {
            return View();
        }
    }
}
