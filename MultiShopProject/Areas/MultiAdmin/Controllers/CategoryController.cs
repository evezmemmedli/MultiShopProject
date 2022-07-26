using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using MultiShopProject.DAL;
using MultiShopProject.Models;
using MultiShopProject.Utilities;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MultiShopProject.Areas.MultiAdmin.Controllers
{
    [Area("MultiAdmin")]
    public class CategoryController : Controller
    {
        private readonly AppDbContext _context;
        private readonly IWebHostEnvironment _env;
        public CategoryController(AppDbContext context, IWebHostEnvironment env)
        {
            _context = context;
            _env = env;
        }
        public IActionResult Index()
        {
            List<Category> category = _context.Categories.ToList();
            return View(category);
        }

        public IActionResult Create()
        {
            return View();
        }
        [HttpPost]
        [AutoValidateAntiforgeryToken]
        public async Task<IActionResult> Create(Category category)
        {
            if (!ModelState.IsValid) return View();
            if (category.Photo is null)
            {
                ModelState.AddModelError("Photo", "You have to choose 1 image at least");
                return View();
            }
            if (!category.Photo.ImageIsOkay(2))
            {
                ModelState.AddModelError("Photo", "Please choose valid image size");
                return View();
            }
            category.Image = await category.Photo.FileCreate(_env.WebRootPath, "assets/img");

            if (!ModelState.IsValid) return View();
            Category existed = _context.Categories.FirstOrDefault(c => c.Name.ToLower().Trim() == category.Name.ToLower().Trim());
            if (existed != null)
            {
                ModelState.AddModelError("Name", "You can not duplicate category name");
                return View();
            }
            _context.Categories.Add(category);
            _context.SaveChanges();
            return RedirectToAction(nameof(Index));
        }
        public IActionResult Update(int? id)
        {
            if (id is null || id == 0) return NotFound();
            Category category = _context.Categories.FirstOrDefault(c => c.Id == id);
            if (category is null) return NotFound();
            return View(category);
        }
        [HttpPost]
        [AutoValidateAntiforgeryToken]
        public async Task<IActionResult> Update(int? id, Category newCategory)
        {
            if (id is null || id == 0) return NotFound();

            Category existed = _context.Categories.FirstOrDefault(c => c.Id == id);
            if (!ModelState.IsValid) return View(existed);
            if (existed == null) return NotFound();
            if (newCategory.Photo == null)
            {
                string fileName = existed.Image;
                _context.Entry(existed).CurrentValues.SetValues(newCategory);
                existed.Image = fileName;

            }
            else
            {
                if (!newCategory.Photo.ImageIsOkay(2))
                {
                    ModelState.AddModelError("Photo", "Please choose valid image size (max : 2mb)");
                    return View(existed);
                }
                FileValidator.FileDelete(_env.WebRootPath, "assets/img", existed.Image);

                _context.Entry(existed).CurrentValues.SetValues(newCategory);
                existed.Image = await newCategory.Photo.FileCreate(_env.WebRootPath, "assets/img");
            }


            _context.SaveChanges();
            return RedirectToAction(nameof(Index));
        }
        public async Task<IActionResult> Delete(int? id)
        {
            if (id is null || id == 0) return NotFound();
            Category category = await _context.Categories.FindAsync(id);
            if (category is null) return NotFound();
            if (category.Image != null)
            {
                FileValidator.FileDelete(_env.WebRootPath, "assets/img", category.Image);


            }
            _context.Categories.Remove(category);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));

        }
    }
}
