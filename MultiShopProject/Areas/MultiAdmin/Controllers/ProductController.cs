using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using MultiShopProject.DAL;
using MultiShopProject.Models;
using MultiShopProject.Utilities;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MultiShopProject.Areas.MultiAdmin.Controllers
{
    [Area("MultiAdmin")]
    public class ProductController : Controller
    {
        private readonly AppDbContext _context;
        private readonly IWebHostEnvironment _env;
        public ProductController(AppDbContext context, IWebHostEnvironment env)
        {
            _context = context;
            _env = env;
        }
        public IActionResult Index()
        {
            List<Product> product = _context.Products
                .Include(p=>p.Category)
                .Include(p=>p.ProductImages)
                .Include(p=>p.ProductInformation)
                .ToList();
            return View(product);
        }

        public IActionResult Create()
        {
            ViewBag.Categories = _context.Categories.ToList();
            ViewBag.ProductInformations = _context.ProductInformations.ToList();
            return View();
        }
        [HttpPost]
        [AutoValidateAntiforgeryToken]
        public async Task<IActionResult> Create(Product product)
        {
            if (!ModelState.IsValid)
            {
                ViewBag.Categories = _context.Categories.ToList();
                ViewBag.ProductInformations = _context.ProductInformations.ToList();
                return View();
            }
            if (product.MainPhoto == null ||  product.Photos == null)
            {
                ViewBag.Categories = _context.Categories.ToList();
                ViewBag.ProductInformations = _context.ProductInformations.ToList();
                ModelState.AddModelError(string.Empty, "You must choose 1 main photo and 1 hover photo and 1 another photo ");
                return View();
            }
            if (!product.MainPhoto.ImageIsOkay(2))
            {
                ViewBag.Categories = _context.Categories.ToList();
                ViewBag.ProductInformations = _context.ProductInformations.ToList();
                ModelState.AddModelError(string.Empty, "Please choose valid image file");
                return View();
            }
           
           
            product.ProductImages = new List<ProductImage>();
            TempData["FileName"] = "";
            List<IFormFile> removeable = new List<IFormFile>();
            foreach (var photo in product.Photos)
            {
                if (!photo.ImageIsOkay(2))
                {
                    removeable.Add(photo);
                    TempData["FileName"] += photo.FileName + " ,";
                    continue;
                }
                ProductImage productImage = new ProductImage
                {
                    IsMain = false,
                    Product = product,
                    Name = await photo.FileCreate(_env.WebRootPath, "assets/img")
                };
                 _context.ProductImages.Add(productImage);
            }
            product.Photos.RemoveAll(p => removeable.Any(r => r.FileName == p.FileName));
            ProductImage mainProductImage = new ProductImage
            {
                IsMain = true,
                Product = product,
                Name = await product.MainPhoto.FileCreate(_env.WebRootPath, "assets/img")
            };
            _context.ProductImages.Add(mainProductImage);


            _context.Products.Add(product);
             _context.SaveChanges();
            return RedirectToAction(nameof(Index));

            
        }

        public async Task<IActionResult> Delete(int? id)
        {
            if (id is null || id == 0) return NotFound();
            Product product = await _context.Products.FindAsync(id);
            if (product is null) return NotFound();
            if (product.MainPhoto != null)
            {
                FileValidator.FileDelete(_env.WebRootPath, "assets/img",product.ProductImages.FirstOrDefault(p=>p.IsMain == true).Name);
               

            }
            if (product.Photos != null)
            {
                FileValidator.FileDelete(_env.WebRootPath, "assets/img", product.ProductImages.FirstOrDefault(p => p.IsMain == false).Name);
            }
            _context.Products.Remove(product);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));

        }
    }
}
