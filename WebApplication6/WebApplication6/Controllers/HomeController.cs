using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Http;
using System;

namespace WebApplication6.Controllers
{
    public class HomeController : Controller
    {
        public IActionResult Index()
        {
            return View("Index");
        }



        [HttpPost]
        public IActionResult SetCookie(string value, DateTime expiryDate)
        {
            var options = new CookieOptions
            {
                Expires = expiryDate,
                HttpOnly = true
            };

            Response.Cookies.Append("MyCookie", value, options);

            
            Console.WriteLine($"Cookie set. Value: {value}, Expiry Date: {expiryDate}");

            return RedirectToAction("Index");
        }

        public IActionResult CheckCookie()
        {
            var cookieValue = Request.Cookies["MyCookie"];

            
            Console.WriteLine($"Checking Cookie. Raw Cookies: {Request.Headers["Cookie"]}");
            Console.WriteLine($"Checking Cookie. Value: {cookieValue}");

            if (string.IsNullOrEmpty(cookieValue))
            {
                ViewData["Message"] = "Cookie not set.";
            }
            else
            {
                ViewData["Message"] = $"Cookie value: {cookieValue}";
            }

            return View();
        }



    }
}
