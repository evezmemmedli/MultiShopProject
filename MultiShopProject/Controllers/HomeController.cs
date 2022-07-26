using Microsoft.AspNetCore.Mvc;
using MultiShopProject.DAL;
using MultiShopProject.Models;
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
            List<Slider> slider = _context.Sliders.ToList();
            return View(slider);
        }
        public IActionResult ContactUs()
        {
            return View();
        }
    }
}
