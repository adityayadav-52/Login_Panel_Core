using FinalLabTest.DB_Context;
using FinalLabTest.Models;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;

namespace FinalLabTest.Controllers
{

    
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;

        public HomeController(ILogger<HomeController> logger)
        {
            _logger = logger;
        }

        [Authorize]

        public IActionResult Index()
        {
            return View();
        }

        [Authorize]

        public IActionResult Privacy()
        {
            return View();
        }


        [HttpGet]
        public IActionResult LogIn()
        {
            return View();
        }

        [HttpPost]
        public IActionResult LogIn(UserReg objUser)
        {
            MytableContext dbobj = new MytableContext();

            var res = dbobj.UserInfos.Where(a => a.Email == objUser.Email).FirstOrDefault();

            if (res == null)
            {

                TempData["Invalid"] = "Email is not found";
            }

            else
            {
                if (res.Email == objUser.Email && res.Password == objUser.Password)
                {

                    var claims = new[] { new Claim(ClaimTypes.Name, res.Name),
                                        new Claim(ClaimTypes.Email, res.Email) };

                    var identity = new ClaimsIdentity(claims, CookieAuthenticationDefaults.AuthenticationScheme);

                    var authProperties = new AuthenticationProperties
                    {
                        IsPersistent = true
                    };
                    HttpContext.SignInAsync(
                    CookieAuthenticationDefaults.AuthenticationScheme,
                    new ClaimsPrincipal(identity),
                    authProperties);


                    HttpContext.Session.SetString("Name", res.Name);

                    return RedirectToAction("Index", "Employee");

                }

                else
                {

                    TempData["Wrong"] = "Wrong Password";

                    return View("Login");
                }


            }


            return View();
        }
        public IActionResult LogOut()
        {
            HttpContext.SignOutAsync(
                CookieAuthenticationDefaults.AuthenticationScheme);

            return RedirectToAction("LogIn", "Home");
        }

        [HttpGet]
        public IActionResult NewUserReg()
        {
            return View();
        }

        [HttpPost]
        public IActionResult NewUserReg(UserReg sobj)
        {
            MytableContext dbobj = new MytableContext();
            UserInfo tbl = new UserInfo();
            tbl.Id = sobj.Id;
            tbl.Name = sobj.Name;
            tbl.Email = sobj.Email;
            tbl.Password = sobj.Password;

            dbobj.UserInfos.Add(tbl);
            dbobj.SaveChanges();
            return RedirectToAction("LogIn", "Home");
        }


    [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
