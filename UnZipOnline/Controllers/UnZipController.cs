using Microsoft.AspNetCore.Components.Forms;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
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
                var lastModified = DateTime.UtcNow;
                var name = Guid.NewGuid().ToString();
                string path = "/File/" + name;
                string fullPath = _environment.WebRootPath + path;

                string targetFolder = _environment.WebRootPath + @"\Extracted\" + $@"{Guid.NewGuid()}\";

                FileModel file = new FileModel();
                if (uploadfile != null)
                {

                    using (var stream = new FileStream(fullPath, FileMode.Create))
                    {
                        await uploadfile.CopyToAsync(stream);
                    }


                }

                string zipFile = fullPath;

                ZipFile.ExtractToDirectory(zipFile, targetFolder);

                string[] extractedFiles = Directory.GetFiles(targetFolder);
                string extractedFile = extractedFiles[0];
                string extractedFileName = Path.GetFileName(extractedFile);

                file = new FileModel { Name = name,ExtractedFileName =extractedFileName, ExtractedFile = extractedFile, 
                    Path = path, ContentType = uploadfile.ContentType, 
                        ExtractedTo = targetFolder, LastModified = lastModified };
                _context.Files.Add(file);
                _context.SaveChanges();

            }
            catch (Exception ex)
            {
                ViewBag.ErrorMessage = ex.Message;
                return View("Message", ViewBag.ErrorMessage);

            }
            return RedirectToAction("Download");
        }

        public async Task<IActionResult> Download()
        {
            var file = await _context.Files.OrderBy(e=>e.LastModified).LastOrDefaultAsync();
            var path = file.ExtractedFile;

            var memory = new MemoryStream();
            using (var stream = new FileStream(path, FileMode.Open))
            {
                await stream.CopyToAsync(memory);
            }
            memory.Position = 0;

            return File(memory, "application/octet-stream", Path.GetFileName(path));

        }

    }
}