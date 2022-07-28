using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using MultiShopProject.DAL;
using MultiShopProject.Models;
using MultiShopProject.Utilities;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MultiShopProject.Areas.MultiAdmin.Controllers
{[Area("MultiAdmin")]
    public class AdvertisementController : Controller
    {
        private readonly AppDbContext _context;

        private readonly IWebHostEnvironment _env;

        public AdvertisementController(AppDbContext context, IWebHostEnvironment env)
        {
            _context = context;
            _env = env;
        }
        public IActionResult Index()
        {
            List<Advertisement> adv = _context.Advertisements.ToList();
            return View(adv);
        }
        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        [AutoValidateAntiforgeryToken]
        public async Task<IActionResult> Create(Advertisement adv)
        {
            if (!ModelState.IsValid) return View();
            if (adv.Photo is null)
            {
                ModelState.AddModelError("Photo", "You have to choose 1 image at least");
                return View();
            }
            if (!adv.Photo.ImageIsOkay(2))
            {
                ModelState.AddModelError("Photo", "Please choose valid image size");
                return View();
            }
            adv.Image = await adv.Photo.FileCreate(_env.WebRootPath, "assets/img");
            await _context.Advertisements.AddAsync(adv);
            await _context.SaveChangesAsync();


            return RedirectToAction(nameof(Index));
        }
        public async Task<IActionResult> Update(int? id)
        {
            if (id == 0 || id is null) return NotFound();
            Advertisement adv = await _context.Advertisements.FirstOrDefaultAsync(s => s.Id == id);
            if (adv == null) return NotFound();
            return View(adv);
        }
        [HttpPost]
        public async Task<IActionResult> Update(int? id, Advertisement adv)
        {
            if (id == 0 || id is null) return NotFound();
            Advertisement existed = await _context.Advertisements.FindAsync(id);
            if (existed is null) return NotFound();
            if (!ModelState.IsValid) return View(existed);
            if (adv.Photo == null)
            {
                string fileName = existed.Image;
                _context.Entry(existed).CurrentValues.SetValues(adv);
                existed.Image = fileName;
                //existed.Title = slider.Title;
                //existed.Article = slider.Article;
                //existed.ButtonUrl = slider.ButtonUrl;
                //existed.Order = slider.Order;
            }
            else
            {
                if (!adv.Photo.ImageIsOkay(2))
                {
                    ModelState.AddModelError("Photo", "Please choose valid image size (max : 2mb)");
                    return View(existed);
                }
                FileValidator.FileDelete(_env.WebRootPath, "assets/img", existed.Image);

                _context.Entry(existed).CurrentValues.SetValues(adv);
                existed.Image = await adv.Photo.FileCreate(_env.WebRootPath, "assets/img");
            }
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == 0 || id is null) return NotFound();
            Advertisement adv = await _context.Advertisements.FirstOrDefaultAsync(s => s.Id == id);
            if (adv == null) return NotFound();
            if (adv.Image != null)
            {
                FileValidator.FileDelete(_env.WebRootPath, "assets/img", adv.Image);


            }
            _context.Advertisements.Remove(adv);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));

        }
    }
}
