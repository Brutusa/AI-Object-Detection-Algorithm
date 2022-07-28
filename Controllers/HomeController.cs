using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using AussieAnimalAi3._1Prototype.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.Net.Http.Headers;
using System.IO;

namespace AussieAnimalAi3._1Prototype.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;

        public HomeController(ILogger<HomeController> logger)
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

    public partial class UploadController : Controller
    {

        public IActionResult BasicUsage()
        {
            return View();
        }


        public ActionResult Submit(IEnumerable<IFormFile> files)
        {
            IEnumerable<string> fileInfo = new List<string>();

            if (files != null)
            {
                fileInfo = GetFileInfo(files);
            }

            return View("Result", fileInfo);
        }


        public ActionResult Result()
        {
            return View();
        }

        private IEnumerable<string> GetFileInfo(IEnumerable<IFormFile> files)
        {
            List<string> fileInfo = new List<string>();

            foreach (var file in files)
            {
                var fileContent = ContentDispositionHeaderValue.Parse(file.ContentDisposition);
                var fileName = Path.GetFileName(fileContent.FileName.ToString().Trim('"'));

                fileInfo.Add(string.Format("{0} ({1} bytes)", fileName, file.Length));
            }

            return fileInfo;
        }
    }

}
