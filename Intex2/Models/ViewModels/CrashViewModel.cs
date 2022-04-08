using Intex2.odels.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Intex2.Models.ViewModels
{
    public class CrashViewModel
    {
        public IQueryable<Crash> Crashes { get; set; }

        public PageInfo PageInfo {get;set;}

        public Crash crash { get; set; }
    }
}
