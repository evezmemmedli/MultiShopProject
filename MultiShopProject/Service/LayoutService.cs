using Microsoft.AspNetCore.Http;
using MultiShopProject.DAL;
using MultiShopProject.Models;
using MultiShopProject.ViewModels;
using Newtonsoft.Json;
using System.Collections.Generic;
using System.Linq;

namespace MultiShopProject.Service
{
    public class LayoutService
    {
        private readonly AppDbContext _context;
        private readonly IHttpContextAccessor _http;

        public LayoutService(AppDbContext context,IHttpContextAccessor http)
        {
            _context = context;
            _http = http;
        }
        public List<Setting> GetSetting()
        {
            List<Setting> settings = _context.Settings.ToList();
            return settings;
        }
        public List<Category> GetCategory()
        {
            List<Category> categories = _context.Categories.ToList();
            return categories;
        }
        public BasketVM GetBasket()
        {
            //BasketVM basket = new BasketVM();
            string basketStr = _http.HttpContext.Request.Cookies["Basket"];
            if (!string.IsNullOrEmpty(basketStr))
            {
               BasketVM  basket= JsonConvert.DeserializeObject<BasketVM>(basketStr);
                foreach(var item in basket.BasketCookieItemVMs)
                {
                    Product existed = _context.Products.FirstOrDefault(p=>p.Id == item.Id);
                    if(existed == null)
                    {
                        basket.BasketCookieItemVMs.Remove(item);
                    }
             
                }
                return basket;
            }
            return null;
        }
    }
}
