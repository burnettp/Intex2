using Intex2.Models;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Intex2.Components
{
    public class CountyViewComponent : ViewComponent
    {
        private CrashContext _context { get; set; }

        public CountyViewComponent(CrashContext temp)
        {
            _context = temp;
        }

        public IViewComponentResult Invoke()
        {
            ViewBag.SelectedCounty = RouteData?.Values["county"];

            var county = _context.Crashes
                .Select(x => x.COUNTY_NAME)
                .Distinct()
                .OrderBy(x => x);

            return View(county);
        }
    }
}
