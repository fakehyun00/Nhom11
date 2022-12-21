using Microsoft.AspNetCore.Mvc;
using Nhom11.Data;
using Nhom11.Models;
using System.Security.Cryptography;
using System.Text;
using Microsoft.AspNetCore.Session;
using Microsoft.AspNetCore.Http;
using Nhom11.Helpers;

namespace Nhom11.Controllers
{
    public class AccountsController : Controller
    {
        private WebQuanAoContext _context;
        public AccountsController(WebQuanAoContext context)
        {
            _context = context;
        }
        public IActionResult Register()
        {
            return View();
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Register(Account account)
        {
            if (ModelState.IsValid)
            {
                var check = _context.Accounts.FirstOrDefault(s => s.Username == account.Username);
                if (check == null)
                {
                    _context.Accounts.Add(account);
                    _context.SaveChanges();
                    return RedirectToAction("Login");
                }
                else
                {
                    ViewBag.error = "Email already exists";
                    return View();
                }


            }
            return View();
        }
        public IActionResult Login()
        {
            return View();
        }
        [HttpPost]
        [ValidateAntiForgeryToken]

        public IActionResult Login(Account account)
        {
            var check=_context.Accounts.Where(x => x.Username.Equals(account.Username) && x.Password.Equals(account.Password)).FirstOrDefault();
            if (check == null)
            {
                account.LoginErrorMessage = "Error Username or Password! Try again please!";
                return View("Login", account);
               
            }
            else
            {
                /* CookieOptions options = new CookieOptions();
                options.Expires=DateTime.Now.AddDays(7);
                HttpContext.Response.Cookies.Append("username", account.Username, options);*/
                //SessionOptions session=new SessionOptions();
                HttpContext.Session.SetString("User",account.Username);
                return RedirectToAction("Index", "Products");
            }
            
        }

        //POST: Register
        
        public static string GetMD5(string str)
        {
            MD5 md5 = new MD5CryptoServiceProvider();
            byte[] fromData = Encoding.UTF8.GetBytes(str);
            byte[] targetData = md5.ComputeHash(fromData);
            string byte2String = null;

            for (int i = 0; i < targetData.Length; i++)
            {
                byte2String += targetData[i].ToString("x2");

            }
            return byte2String;
        }
        public IActionResult LogOut()
        {
            HttpContext.Session.Clear();
            return RedirectToAction("Login","Accounts");
        }
    }

}
