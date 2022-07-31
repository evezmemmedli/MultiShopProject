using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using MultiShopProject.DAL;
using MultiShopProject.Models;
using MultiShopProject.ViewModels;
using Newtonsoft.Json;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MultiShopProject.Controllers
{
    public class ProductController : Controller
    {
        private readonly AppDbContext _context;

        public ProductController(AppDbContext context)
        {
            _context = context;
        }
        public async Task<IActionResult> Index(int? id, Sort? sort)
        {
            if (id is null || id == 0) return NotFound();

            List<Product> products = new List<Product>();

            var productQuery = _context.Products.Include(p => p.ProductImages)
                .Where(c => c.CategoryId == id).AsQueryable();

            switch (sort)
            {
                case Sort.AscName:
                    productQuery = productQuery.OrderBy(x => x.Name);
                    break;
                case Sort.DescName:
                    productQuery = productQuery.OrderByDescending(x => x.Name);
                    break;
                case Sort.AscPrice:
                    productQuery = productQuery.OrderBy(x => x.Price);
                    break;
                case Sort.DescPrice:
                default:
                    productQuery = productQuery.OrderByDescending(x => x.Price);
                    break;
            }
            products = await productQuery.ToListAsync();
            return View(products);
        }
        public IActionResult Detail(int? id)
        {

            if (id is null || id == 0) return NotFound();

            List<Product> products = _context.Products.Include(p => p.ProductImages)
                .Include(p => p.Category)
                .Include(p => p.ProductInformation)
                .ToList();
            return View(products);

        }

        public async Task<IActionResult> AddBasket(int? id)
        {
            if(id is null || id==0) return NotFound();

            Product product = await _context.Products.FirstOrDefaultAsync(p => p.Id == id);
            if(product == null) return NotFound();
            string basketStr = HttpContext.Request.Cookies["Basket"];
            BasketVM basket = new BasketVM();
            if (string.IsNullOrEmpty(basketStr))
            {
                BasketCookieItemVM cookieItem = new BasketCookieItemVM
                {
                    Id = product.Id,
                    Quantity = 1
                };
                basket.BasketCookieItemVMs = new List<BasketCookieItemVM>();
                basket.BasketCookieItemVMs.Add(cookieItem);
                basket.TotalPrice = product.Price;
            }
            else
            {
                basket = JsonConvert.DeserializeObject<BasketVM>(basketStr);
                BasketCookieItemVM existed = basket.BasketCookieItemVMs.Find(p => p.Id == id);
                if(existed == null)
                {
                    BasketCookieItemVM cookieItem = new BasketCookieItemVM
                    {
                        Id = product.Id,
                        Quantity = 1
                    };
                    basket.BasketCookieItemVMs.Add(cookieItem);
                    basket.TotalPrice+=product.Price;
                }
                else
                {
                    basket.TotalPrice += product.Price;
                    existed.Quantity++;
                }
            }
         
            
             basketStr = JsonConvert.SerializeObject(basket);
            HttpContext.Response.Cookies.Append("Basket", basketStr);
            return RedirectToAction("Index","Home");
        }
        public IActionResult ShowBasket()
        {
            if (HttpContext.Request.Cookies["Basket"] == null) return NotFound();
            BasketVM basket = JsonConvert.DeserializeObject<BasketVM>(HttpContext.Request.Cookies["Basket"]);
            return Json(basket);
        }
    }
}
