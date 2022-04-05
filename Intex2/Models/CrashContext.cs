using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;

namespace Intex2.Models
{
    public class CrashContext : DbContext
    {
        public CrashContext(DbContextOptions<CrashContext> options) : base(options)
        {

        }

        public DbSet<Crash> Crashes { get; set; }
    }
}
