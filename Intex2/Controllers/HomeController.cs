using Intex2.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;

namespace Intex2.Controllers
{
    public class HomeController : Controller
    {   
        private CrashContext _context { get; set; }

        public HomeController(CrashContext temp)
        {
            _context = temp;
        }
            
        public IActionResult Login()
        {
            return View();
        }

        public IActionResult Index()
        {

            var CrashInfo = _context.Crashes.Where(x => x.CRASH_ID == "11281387").ToList();
            return View(CrashInfo);
        }

        public IActionResult Crash()
        {
            return View();
        }
        public IActionResult Analyses()
        {
            return View();
        }
        public IActionResult Summary()
        {
            return View();
        }
    }
}
