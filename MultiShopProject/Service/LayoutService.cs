using MultiShopProject.DAL;
using MultiShopProject.Models;
using System.Collections.Generic;
using System.Linq;

namespace MultiShopProject.Service
{
    public class LayoutService
    {
        private readonly AppDbContext _context;

        public LayoutService(AppDbContext context)
        {
            _context = context;
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
    }
}
