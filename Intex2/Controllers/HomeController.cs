using Intex2.Models;
using Intex2.Models.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Microsoft.ML.OnnxRuntime;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;

namespace Intex2.Controllers
{
    // Requires Admin or Reader role to view anything in Home Controller
    [Authorize(Roles = "Admin, Reader")]
    public class HomeController : Controller
    {   
        private CrashContext _context { get; set; }

        public HomeController(CrashContext temp)
        {
            _context = temp;
        }

        // Allows unauthenticated users to view the Index page
        [AllowAnonymous]
        public IActionResult Index()
        {
            return View();
        }
        
        public IActionResult Crash(string CrashID)
        {

            // Find the correct Crash to view details
            var crashdetail = _context.Crashes.Where(x => x.CRASH_ID == CrashID).ToList();

            return View(crashdetail);
        }

        // Allow anyone to create an account with Reader role (role assigned in /Areas/Pages/Account/Register.cshtml
        [AllowAnonymous]
        public IActionResult Register()
        {
            return View();
        }

        public IActionResult Analyses()
        {
            return View();
        }

        [HttpGet]
        public IActionResult Summary(string county,int pageNum = 1)
        {
            //Make Pagination
            int pageSize = 100;

            var x = new CrashViewModel
            {
                Crashes = _context.Crashes
                .Where(x => x.COUNTY_NAME == county || county == null)
                .OrderBy(x => x.CRASH_ID)
                .Skip((pageNum - 1) * pageSize)
                .Take(pageSize),

                PageInfo = new PageInfo
                {
                    TotNumCarIncidents = 
                    (county == null 
                        ? _context.Crashes.Count() 
                        : _context.Crashes.Where(x => x.COUNTY_NAME == county).Count()),
                    AccidentsPerPage = pageSize,
                    CurrentPage = pageNum
                }

            };

            return View(x);
        }

        [HttpPost]
        public IActionResult Summary(string searchedCrash)
        {
            var specificCrash = _context.Crashes.Where(x => x.CRASH_ID == searchedCrash).ToList();

            return View("SearchResults", specificCrash);
        }

        public IActionResult Confirmation()
        {
            
            return View();
        }

        // Only Admin can create
        [Authorize(Roles = "Admin")]
        [HttpGet]
        public IActionResult Create()
        {
            var x = new Crash();

            var CrashInfo = _context.Crashes.ToList();

            
            // This creates a new index without causing issues
            ViewBag.NewIndex = CrashInfo.Count + 1;

            return View(x);
        }

        [Authorize(Roles = "Admin")]
        [HttpPost]
        public IActionResult Create(Crash c)
        {
            if (ModelState.IsValid)
            {
                _context.Crashes.Add(c);
                _context.SaveChanges();

                string idNumber = c.CRASH_ID;

                return RedirectToAction("Confirmation");
            }
            else
            {
                return RedirectToAction("Index");
            }
        }

        [Authorize(Roles = "Admin")]
        [HttpGet]
        public IActionResult Delete(string crashID)
        {

            var specificCrash = _context.Crashes.Single(x => x.CRASH_ID == crashID);


            return View(specificCrash);
        }

        [Authorize(Roles = "Admin")]
        [HttpPost]
        public IActionResult Delete(Crash crashDeleted)
        {
            _context.Crashes.Remove(crashDeleted);
            _context.SaveChanges();

            return RedirectToAction("Summary");
        }

        [Authorize(Roles = "Admin")]
        [HttpGet]
        public IActionResult Edit(string crashID)
        {
            var specificCrash = _context.Crashes.Single(x => x.CRASH_ID == crashID);

            return View(specificCrash);
        }

        [Authorize(Roles = "Admin")]
        [HttpPost]
        public IActionResult Edit(Crash changedCrash)
        {
            _context.Crashes.Update(changedCrash);
            _context.SaveChanges();

            return RedirectToAction("Summary");
        }

        public IActionResult SearchResults(string crashid)
        {
            var specificCrash = _context.Crashes.Where(x => x.CRASH_ID == crashid).ToList();

            return View(specificCrash);
        }

        //Privacy policy page
        public IActionResult PrivacyPolicy()
        {
            return View();
        }
    }
}
