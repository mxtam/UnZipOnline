using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;
using UnZipOnline.Models;

namespace UnZipOnline.Controllers
{
    public class UnZipController : Controller
    {
        private readonly ILogger<UnZipController> _logger;

        public UnZipController(ILogger<UnZipController> logger)
        {
            _logger = logger;
        }

        public IActionResult Index()
        {
            return View();
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