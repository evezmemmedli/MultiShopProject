using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using MultiShopProject.DAL;
using MultiShopProject.Models;
using MultiShopProject.Utilities;
using System;
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
                .Include(p => p.Category)
                .Include(p => p.ProductImages)
                .Include(p => p.ProductInformation)
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
        public async Task<IActionResult> Create(Product productForCreate)
        {
            if (!ModelState.IsValid)
            {
                ViewBag.Categories = _context.Categories.ToList();
                ViewBag.ProductInformations = _context.ProductInformations.ToList();
                return View();
            }
            if (productForCreate.MainPhoto == null || productForCreate.Photos == null)
            {
                ViewBag.Categories = _context.Categories.ToList();
                ViewBag.ProductInformations = _context.ProductInformations.ToList();
                ModelState.AddModelError(string.Empty, "You must choose 1 main photo and 1 hover photo and 1 another photo ");
                return View();
            }
            if (!productForCreate.MainPhoto.ImageIsOkay(2))
            {
                ViewBag.Categories = _context.Categories.ToList();
                ViewBag.ProductInformations = _context.ProductInformations.ToList();
                ModelState.AddModelError(string.Empty, "Please choose valid image file");
                return View();
            }


            productForCreate.ProductImages = new List<ProductImage>();
            TempData["FileName"] = "";
            List<IFormFile> removeable = new List<IFormFile>();
            foreach (var photo in productForCreate.Photos)
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
                    Product = productForCreate,
                    Name = await photo.FileCreate(_env.WebRootPath, "assets/img")
                };
                _context.ProductImages.Add(productImage);
            }
            productForCreate.Photos.RemoveAll(p => removeable.Any(r => r.FileName == p.FileName));
            ProductImage mainProductImage = new ProductImage
            {
                IsMain = true,
                Product = productForCreate,
                Name = await productForCreate.MainPhoto.FileCreate(_env.WebRootPath, "assets/img")
            };
            _context.ProductImages.Add(mainProductImage);


            _context.Products.Add(productForCreate);
            _context.SaveChanges();
            return RedirectToAction(nameof(Index));
        }

        public async Task<IActionResult> Update(int? id)
        {
            if (id is null || id == 0) return NotFound();

            ViewBag.ProductionInformations = _context.ProductInformations.ToList();
            ViewBag.Categories = _context.Categories.ToList();

            Product product = await _context.Products
                .Include(p => p.ProductImages)
                .Include(p => p.ProductInformation)
                .SingleOrDefaultAsync(p => p.Id == id);

            if (product == null) return NotFound();

            return View(product);
        }

        [HttpPost]
        [AutoValidateAntiforgeryToken]
        public async Task<IActionResult> Update(int id, Product productForUpdate)
        {
            if (id == 0) return NotFound();

            Product existed = await _context.Products.Include(p => p.ProductImages)
                .Include(p => p.ProductInformation).FirstOrDefaultAsync(p => p.Id == id);
            if (existed == null) return NotFound();

            existed.ProductInformation.Name = productForUpdate.ProductInformation.Name;
            existed.Desc = productForUpdate.Desc;
            existed.Price = productForUpdate.Price;
            existed.CategoryId = productForUpdate.CategoryId;

            if (productForUpdate.MainPhoto != null)
            {
                if (!productForUpdate.MainPhoto.ImageIsOkay(2))
                {
                    ModelState.AddModelError("","test");
                    return View();
                }
                FileValidator.FileDelete("", _env.WebRootPath, existed.ProductImages.FirstOrDefault(x=>x.IsMain==true).Name);

                ProductImage removMain = existed.ProductImages.FirstOrDefault(x => x.IsMain == true);
                _context.ProductImages.Remove(removMain);
                ProductImage productImage = new ProductImage
                {
                    IsMain=true,
                    ProductId=existed.Id,
                    Name=await FileValidator.FileCreate(productForUpdate.MainPhoto,_env.WebRootPath,"assets/img")
                };
                existed.ProductImages.Add(productImage);
                

            }



            _context.SaveChanges();

            //_context.Update(existed);
            //int updated = _context.SaveChanges();

            //if (updated == 0)
            //{
            //    throw new Exception("Update is failed");
            //}

            return Ok();
        }

        public async Task<IActionResult> Delete(int? id)
        {
            if (id is null || id == 0) return NotFound();
            Product product = await _context.Products.FindAsync(id);
            if (product is null) return NotFound();
            if (product.MainPhoto != null)
            {
                FileValidator.FileDelete(_env.WebRootPath, "assets/img", product.ProductImages.FirstOrDefault(p => p.IsMain == true).Name);


            }
            if (product.Photos != null)
            {
                FileValidator.FileDelete(_env.WebRootPath, "assets/img", product.ProductImages.FirstOrDefault(p => p.IsMain == false).Name);
            }
            _context.Products.Remove(product);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));

        }

        public IActionResult Detail(int? id)
        {
            if (id is null || id == 0) return NotFound();


            Product product =  _context.Products
                .Include(p => p.ProductImages)
                .Include(p => p.ProductInformation)
                .SingleOrDefault(p => p.Id == id);

            if (product == null) return NotFound();

            return View(product);
        }
    }
}
