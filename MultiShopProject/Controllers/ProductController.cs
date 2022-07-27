using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using MultiShopProject.DAL;
using MultiShopProject.Models;
using System.Collections.Generic;
using System.Linq;

namespace MultiShopProject.Controllers
{
    public class ProductController : Controller
    {
        private readonly AppDbContext _context;

        public ProductController(AppDbContext context)
        {
            _context = context;
        }
        public IActionResult Index(int?id)
        {

            if (id is null || id == 0) return NotFound();

            List<Product> products = _context.Products.Include(p => p.ProductImages)
                .Where(c => c.CategoryId == id).ToList();

            return View(products);
        }
        public IActionResult Detail()
        {
            return View();
        }
    }
}
