using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using PhotoGuru_Project.Data;
using PhotoGuru_Project.Models;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace PhotoGuru_Project.Controllers
{
    [Authorize]
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly DBContext _db;

        public HomeController(ILogger<HomeController> logger, DBContext db)
        {
            _logger = logger;
            _db = db;
        }

        public IActionResult Index()
        {
            var images = _db.Images.ToList();
            return View(images);
        }

        public IActionResult UploadPicture()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> UploadPicture(Images img)
        {
            if (ModelState.IsValid)
            {
                var files = HttpContext.Request.Form.Files;
                if (files.Count() > 0)
                {
                    byte[] image = null;
                    using (var filestream = files[0].OpenReadStream())
                    {
                        using (var memorystream = new MemoryStream())
                        {
                            filestream.CopyTo(memorystream);
                            image = memorystream.ToArray();
                        }
                    }
                    img.Picture = image;
                }
                _db.Images.Add(img);
                await _db.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(img);
        }

        public async Task<IActionResult> SearchPicture()
        {
            return View();
        }

        public async Task<IActionResult> ShowSearchResults(String SearchPhrase)
        {
            //return "You entered " + SearchPhrase;
            return View("Index", await _db.Images.Where(i => i.PicName.Contains
            (SearchPhrase)).ToListAsync());
        }

        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]

        public async Task<IActionResult>DeleteConfirm(int id)
        {
            var images = await _db.Images.FindAsync(id);
            _db.Images.Remove(images);
            await _db.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        public async Task<IActionResult> Delete(int? id)
        {
            if(id == null)
            {
                return NotFound();
            }

            var image = await _db.Images.FirstOrDefaultAsync(m => m.Id == id);

            if(image == null)
            {
                return NotFound();
            }

            return View(image);
        }

    }
}
