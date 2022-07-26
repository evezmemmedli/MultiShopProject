﻿using MultiShopProject.Models;
using System.Collections.Generic;

namespace MultiShopProject.ViewModels
{
    public class HomeVM
    {
        public List<Slider> Sliders { get; set; }
        public List<Category> Categories { get; set; }
        public List<Product> Products { get; set; }

        public List<Advertisement>Advertisements { get; set; }
    }
}
