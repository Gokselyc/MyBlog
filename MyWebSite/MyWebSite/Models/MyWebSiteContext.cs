using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MyWebSite.Models
{
    public class MyWebSiteContext : DbContext
    {
        public MyWebSiteContext(DbContextOptions<MyWebSiteContext> options)
            : base(options)
        {
        }

        public DbSet<Blog> Blog { get; set; }
    }
}
