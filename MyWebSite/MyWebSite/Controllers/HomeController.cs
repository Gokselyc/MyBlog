using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using MyWebSite.Models;

namespace MyWebSite.Controllers
{
    public class HomeController : Controller
    {
        string connectionString = "Data Source=DESKTOP-KR782F0;Initial Catalog=BlogDB;Integrated Security=True";


        public IActionResult Index()
        {
            return View();
        }

    }
}
