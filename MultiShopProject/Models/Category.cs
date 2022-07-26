using MultiShopProject.Models.Base;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace MultiShopProject.Models
{
    public class Category:BaseEntity
    {
        [Required,StringLength(20)]

        public string Name { get; set; }    

        public List<Product> Product { get; set; }
    }
}
