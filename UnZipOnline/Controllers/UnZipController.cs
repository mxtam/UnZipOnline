using Microsoft.AspNetCore.Components.Forms;
using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;
using System.IO;
using System.IO.Compression;
using System.Net.WebSockets;
using System.Text.RegularExpressions;
using UnZipOnline.Models;

namespace UnZipOnline.Controllers
{
    public class UnZipController : Controller
    {
        private readonly UnZipContext _context;
        public IWebHostEnvironment _environment;

        public UnZipController(UnZipContext context, IWebHostEnvironment environment)
        {
            _context = context;
            _environment = environment; ;
        }

        public IActionResult Index()
        {
            return View();
        }

        [HttpGet]
        public IActionResult Upload()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Upload(IFormFile uploadfile)
        {
            try
            {
                FileModel file = new FileModel();
                if (uploadfile != null)
                {
                    var name = Guid.NewGuid().ToString();
                    string path = "/File/" +name;

                    using (var stream = new FileStream(_environment.WebRootPath + path, FileMode.Create))
                    {
                        await uploadfile.CopyToAsync(stream);
                    }

                    file = new FileModel { Name = name, Path = path };
                    _context.Files.Add(file);
                    _context.SaveChanges();
                }

                string targetFolder = _environment.WebRootPath + @"\Extracted\";

                string zipFile = _environment.WebRootPath + file.Path;

                ZipFile.ExtractToDirectory(zipFile, targetFolder);

                ExtractedModel extracted = new ExtractedModel();
                string[] files = Directory.GetFiles(targetFolder);
                var extractedFile = files[0];

                extracted = new ExtractedModel { Name = extractedFile };

                _context.Extracted.Add(extracted);
                _context.SaveChanges();
            }
            catch (Exception ex)
            {
                ViewBag.ErrorMessage = ex.Message;
                return View("Message",ViewBag.ErrorMessage);

            }
            return RedirectToAction("Download");
        }



        //[HttpPost]
        public IActionResult Download(string fileName)
        {

            string path = _environment.WebRootPath + @"\Extracted\";
            string[] files = Directory.GetFiles(path);
            var file = files[0];
            byte[] fileBytes = System.IO.File.ReadAllBytes(file);

            
            string uniqueFileName = $"{Guid.NewGuid()}.docx";
            return File(fileBytes, "application/octet-stream", uniqueFileName);
            
        }

    }
}