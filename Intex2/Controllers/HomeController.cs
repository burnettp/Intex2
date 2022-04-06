using Intex2.Models;
using Intex2.Models.ViewModels;
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
            return View();
        }

        public IActionResult Crash(string CrashID)
        {

            // Find the correct Crash to view details
            var crashdetail = _context.Crashes.Where(x => x.CRASH_ID == CrashID).ToList();

            return View(crashdetail);
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

            

            // Pass into the viewbag what level of clearace someone has.
            // Conditional statements used in summary page to determine what is displayed

            // if the user is an admin
            ViewBag.user = 1;

            // if the user is just a reader
            // ViewBag.user = 2;

            return View(x);
        }

        [HttpPost]
        public IActionResult Summary(string searchedCrash)
        {
            var specificCrash = _context.Crashes.Where(x => x.CRASH_ID == searchedCrash).ToList();


            // if the user is an admin
            ViewBag.user = 1;

            // if the user is just a reader
            // ViewBag.user = 2;

            return View("SearchResults", specificCrash);
        }

        public IActionResult Confirmation()
        {
            
            return View();
        }




        [HttpGet]
        public IActionResult Create()
        {
            var x = new Crash();

            var CrashInfo = _context.Crashes.ToList();

            // This creates a new index without causing issues
            ViewBag.NewIndex = CrashInfo.Count + 1;


            return View(x);
        }

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


        [HttpGet]
        public IActionResult Delete(string crashID)
        {

            var specificCrash = _context.Crashes.Single(x => x.CRASH_ID == crashID);


            return View(specificCrash);
        }

        [HttpPost]
        public IActionResult Delete(Crash crashDeleted)
        {
            _context.Crashes.Remove(crashDeleted);
            _context.SaveChanges();

            return RedirectToAction("Summary");
        }

        [HttpGet]
        public IActionResult Edit(string crashID)
        {
            var specificCrash = _context.Crashes.Single(x => x.CRASH_ID == crashID);

            return View(specificCrash);
        }

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
    }
}
