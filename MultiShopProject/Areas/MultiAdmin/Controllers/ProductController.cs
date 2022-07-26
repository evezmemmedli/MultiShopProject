using Microsoft.AspNetCore.Hosting;
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

            //ProductInformation productInformation = new ProductInformation
            //{
            //    Description = product.ProductInformation.Description,
            //    Information = product.ProductInformation.Information
            //};
            ProductImage mainProductImage = new ProductImage
            {
                IsMain = true,
                Product = product,
                Name = await product.MainPhoto.FileCreate(_env.WebRootPath, "assets/img")
            };
             _context.ProductImages.Add(mainProductImage);
            foreach (var photo in product.Photos)
            {
                ProductImage productImage = new ProductImage
                {
                    IsMain = false,
                    Product = product,
                    Name = await photo.FileCreate(_env.WebRootPath, "assets/img")
                };
                 _context.ProductImages.Add(productImage);
            }

             //_context.ProductInformations.Add(productInformation);
             _context.Products.Add(product);
             _context.SaveChanges();
            return RedirectToAction(nameof(Index));
        }
    }
}
