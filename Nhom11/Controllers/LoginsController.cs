using Microsoft.AspNetCore.Mvc;
namespace Nhom11.Controllers
{
    public class LoginsController : Controller
    {
        public IActionResult Index()
        {
            ViewBag.Username = HttpContext.Session.GetString("Username");
            return View();
        }
        
    }
}
