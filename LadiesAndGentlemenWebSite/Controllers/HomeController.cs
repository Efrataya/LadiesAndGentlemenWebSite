using LadiesAndGentlemenWebSite.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;

namespace LadiesAndGentlemenWebSite.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        int i;
        public HomeController(ILogger<HomeController> logger)
        {
            _logger = logger;
            i = 4;
        }

        public IActionResult Index()
        {

            //NewMethod();

            if (HttpContext.Session.GetString("foo") == null)
            {
                HttpContext.Session.SetString("foo", "bar");
            }
            i++;
            return View();



        }




        //private void NewMethod()
        //{
        //    if (HttpContext.Session.GetString("foo") == null)
        //    {
        //        HttpContext.Session.SetString("foo", "bar");
        //    }
        //    i++;

        //}

        public IActionResult Privacy()
        {
            return View();
        }
        public IActionResult about()
        {
            return View();
        }

        public IActionResult contcs()
        {
            return View();
        }
        public IActionResult LogIn()
        {
            return View();
        }
        public IActionResult sending()
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
