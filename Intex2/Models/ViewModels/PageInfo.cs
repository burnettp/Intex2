using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Intex2.Models.ViewModels
{
    public class PageInfo
    {
        public int TotNumCarIncidents { get; set; }

        public int AccidentsPerPage { get; set; }

        public int CurrentPage { get; set; }

        public int TotalPages => (int)Math.Ceiling((double)TotNumCarIncidents / AccidentsPerPage);
    }
}
