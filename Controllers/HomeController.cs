using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Dynamic;
using System.Linq;
using System.Threading.Tasks;
using TestingAplikasi.DAO;
using TestingAplikasi.Models;

namespace TestingAplikasi.Controllers
{
    [Authorize]
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        HomeDAO dao;
        public HomeController(ILogger<HomeController> logger)
        {
            _logger = logger;
            dao = new HomeDAO();
        }

        public IActionResult Index()
        {
            dynamic objek = new ExpandoObject();
            var data = dao.getKaryawanAll();

            objek.table = data;

            return View(objek);
        }

        public IActionResult Detail(string npp)
        {
            dynamic objek = new ExpandoObject();
            objek.npp = npp;
            objek.id = "01";

            return View(objek);
        }

        
        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
