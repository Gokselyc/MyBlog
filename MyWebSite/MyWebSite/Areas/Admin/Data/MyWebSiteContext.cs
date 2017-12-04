using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;

namespace MyWebSite.Models
{
    public class MyWebSiteContext : DbContext
    {
        public MyWebSiteContext (DbContextOptions<MyWebSiteContext> options)
            : base(options)
        {
        }

        public DbSet<MyWebSite.Models.Blog> Blog { get; set; }
    }
}
