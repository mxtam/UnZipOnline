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

                Archives archive = new Archives { Name = name, ContentType = uploadfile.ContentType, LastModified = lastModified };

                _context.Add(archive);
                _context.SaveChanges();


                string fullPath = _environment.WebRootPath + @"\File\" + archive.Name;

               
                string targetFolder = _environment.WebRootPath + @"\Extracted\" + $@"{Guid.NewGuid()}\";


                if (uploadfile != null)
                {

                    using (var stream = new FileStream(fullPath, FileMode.Create))
                    {
                        await uploadfile.CopyToAsync(stream);
                    }


                }

                string zipFile = fullPath;

                ZipFile.ExtractToDirectory(zipFile, targetFolder);

                Files file = new Files();
                string[] files = Directory.GetFiles(targetFolder);
                foreach (var exFile in files)
                {
                    file = new Files
                    {
                        Name = Path.GetFileName(exFile),
                        ExtractedTo = targetFolder, 
                        LastModified = DateTime.UtcNow,
                        ArchivesId = archive.Id,
                        FullPath= exFile
                       
                    };

                    _context.Files.Add(file);
                }

                _context.SaveChanges();



                return RedirectToAction("Download", archive);

            }
            catch (Exception ex)
            {
                ViewBag.ErrorMessage = ex.Message;
                return View("Message", ViewBag.ErrorMessage);

            }

        }


        [HttpGet]
        public async Task<IActionResult> Download(Archives archives)
        {
            var files = _context.Files.Where(x => x.ArchivesId == archives.Id).ToList();
            
            return View(files);
        }

        [Route("/Dowmnload/{filename}")]
        public async Task<IActionResult> Download(MemoryStream _memory, string filename)
        {
            var file =  _context.Files.FirstOrDefault(x=>x.Name==filename);
            var path = file.FullPath;

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