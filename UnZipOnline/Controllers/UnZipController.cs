using Microsoft.AspNetCore.Components.Forms;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Diagnostics;
using System.IO;
using System.IO.Compression;
using System.Linq;
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
                string fullPath = _environment.WebRootPath + "/File/" + name;

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

                string extractedFiles = "";

                string[] files = Directory.GetFiles(targetFolder);
                foreach (var exFile in files)
                {
                    extractedFiles += Path.GetFileName(exFile) + "&&&";
                }

                file = new FileModel { Name = name, ContentType = uploadfile.ContentType,
                    ExtractedTo = targetFolder, LastModified = lastModified, ExtractedFiles = extractedFiles };
                _context.Files.Add(file);
                _context.SaveChanges();

                return RedirectToAction("Download",file);

            }
            catch (Exception ex)
            {
                ViewBag.ErrorMessage = ex.Message;
                return View("Message", ViewBag.ErrorMessage);

            }
            
        }


        [HttpGet]
        public async Task<IActionResult> Download(FileModel file)
        {
            var _file = file;

            string filestring = file.ExtractedFiles;

            string[] files = filestring.Split("&&&");

            ViewBag.FileName = files;
            return View(ViewBag.FileName);
        }

        [Route("/Dowmnload/{filename}")]
        public async Task<IActionResult> Download(MemoryStream _memory, string filename)
        {
            var file = await _context.Files.FirstOrDefaultAsync(e => e.ExtractedFiles.Contains(filename));
            var path = file.ExtractedTo + filename;

            var memory = _memory;
            using (var stream = new FileStream(path, FileMode.Open))
            {
                await stream.CopyToAsync(memory);
            }
            memory.Position = 0;

            return File(memory, "application/octet-stream", Path.GetFileName(path));

        }

    }
}