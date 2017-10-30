using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using SharpRepository.CoreMvc.Models;
using Microsoft.Extensions.Caching.Memory;
using SharpRepository.Repository;

namespace SharpRepository.CoreMvc.Controllers
{
    public class HomeController : Controller
    {
        protected ITitle title;

        public HomeController(ITitle title)
        {
            this.title = title;
        }

        public IActionResult Index()
        {
            return View();
        }

        public IActionResult About()
        {
            ViewData["Message"] = title.GetTitle() + ": Your application description page.";

            return View();
        }

        public IActionResult Contact()
        {
            ViewData["Message"] = title.GetTitle() + ": Your contact page.";

            return View();
        }

        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
