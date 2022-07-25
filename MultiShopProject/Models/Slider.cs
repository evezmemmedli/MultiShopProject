﻿using MultiShopProject.Models.Base;
using System.ComponentModel.DataAnnotations;

namespace MultiShopProject.Models
{
    public class Slider:BaseEntity
    {
        public string Image { get; set; }
        [Required]
        public string Title { get; set; }
        [Required]
        public string Description { get; set; }
        [Required]
        public string ButtonUrl { get; set; }
        [Required]
        public byte Order { get; set; }
    }
}