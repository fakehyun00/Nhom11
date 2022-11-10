using Microsoft.AspNetCore.Mvc;

namespace Nhom11.Controllers
{
    public class ContactsController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
