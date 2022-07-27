using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using MultiShopProject.DAL;
using MultiShopProject.Models;
using MultiShopProject.ViewModels;
using System.Collections.Generic;
using System.Linq;

namespace MultiShopProject.Controllers
{
    public class HomeController : Controller
    {
        private readonly AppDbContext _context;

        public HomeController(AppDbContext context)
        {
            _context = context;
        }
        public IActionResult Index()
        {

            HomeVM model = new HomeVM()
            {
                Sliders = _context.Sliders.ToList(),
                Categories = _context.Categories.ToList(),
                Products = _context.Products.
                Include(p=>p.ProductImages).
                ToList(),
            };
            return View(model);
        }
        public IActionResult ContactUs()
        {
            return View();
        }
    }
}
