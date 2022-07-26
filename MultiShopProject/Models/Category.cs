using Microsoft.AspNetCore.Http;
using MultiShopProject.Models.Base;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace MultiShopProject.Models
{
    public class Category:BaseEntity
    {
        [Required,StringLength(20)]

        public string Name { get; set; }    
        public string Image { get; set; }

        public List<Product> Product { get; set; }

        [NotMapped]
        public IFormFile Photo { get; set; }
    }
}
