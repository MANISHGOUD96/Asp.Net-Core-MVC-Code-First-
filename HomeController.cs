using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using MK_Core_MVC.DB_Connection;
using MK_Core_MVC.Migrations;
using MK_Core_MVC.Models;
using System.Diagnostics;
using System.Security.Claims;

namespace MK_Core_MVC.Controllers
{
    [Authorize]
    public class HomeController : Controller
    {
        // Declare database 

        private readonly Table _DB;
        public HomeController(Table DB)
        {
            _DB = DB;
        }

        // Read the value of Table 
        public IActionResult Index()
        {
            var res= _DB.Employees.ToList();

            // 4th Step of using Session
            HttpContext.Session.SetString("Name", "MVC-Core First Projact");
            // 5th step of using Session
            var ses=HttpContext.Session.GetString("Name");

            return View(res);
        }

        // Insert value in Table

        [HttpGet]
        public IActionResult AddEmp()
        {
            return View();
        }

        [HttpPost]
        public IActionResult AddEmp(Employee obj)
        {
            if (obj.Id == 0)
            {
                _DB.Employees.Add(obj);
                _DB.SaveChanges();
            }
            else
            {
                _DB.Employees.Update(obj);
                _DB.SaveChanges();  
            }
            return RedirectToAction("Index");
        }

        // Edit table value

        public IActionResult Edit(int Id)
        {
            var resEdit = _DB.Employees.Where(a => a.Id == Id).FirstOrDefault();
            return View("AddEmp",resEdit);
        }

        // Delete table value
        public IActionResult Delete(int Id)
        {
            var resdelete=_DB.Employees.Where(a=>a.Id==Id).First();
            _DB.Employees.Remove(resdelete);
            _DB.SaveChanges();  
            return RedirectToAction("Index");
        }

        // Login Method

        [AllowAnonymous]
        [HttpGet]
        public IActionResult Login()
        {
            return View();
        }

        [AllowAnonymous]
        [HttpPost]
        public IActionResult Login(Login obj)
        {
            var val=_DB.Logins.Where(m=>m.Email==obj.Email).FirstOrDefault();
            if (val==null)
            {
                HttpContext.Session.SetString("Email", "Email is Invalid................!");
                HttpContext.Session.GetString("Email");
            }
            else
            {
                if (val.Email == obj.Email && val.password == obj.password)
                {
                    // Step-1 for Auth
                    var Claims = new[]{ new Claim(ClaimTypes.Name, val.Email), 
                                        new Claim(ClaimTypes.Email,val.password)};
                    // Step-2 for Auth
                    var identity =new ClaimsIdentity(Claims,CookieAuthenticationDefaults.AuthenticationScheme);

                    var authProperties = new AuthenticationProperties
                    {
                        IsPersistent = true
                    };
                    // Step-3 for Auth
                    HttpContext.SignInAsync(
                        CookieAuthenticationDefaults.AuthenticationScheme,
                        new ClaimsPrincipal(identity), 
                        authProperties);

                    HttpContext.Session.SetString("Email", val.Email);
                    HttpContext.Session.GetString("Email");

                    return RedirectToAction("Index");
                }
                else
                {
                    HttpContext.Session.SetString("Password", "Password is Invalid................!");
                    HttpContext.Session.GetString("Password");
                }
            }
            return View();
        }

        // LogOut Method
        public IActionResult LogOut()
        {
            HttpContext.SignOutAsync(null, null);
            HttpContext.Session.Clear();
            return View("Login");
        }
        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}