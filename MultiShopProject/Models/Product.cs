using Microsoft.AspNetCore.Http;
using MultiShopProject.Models.Base;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace MultiShopProject.Models
{
    public class Product : BaseEntity
    {
        [Required]
        public string Name { get; set; }
        [Required]
        public decimal Price { get; set; }
        [Required]
        public string Desc { get; set; }
        public int ProductInformationId { get; set; }
        public ProductInformation ProductInformation { get; set; }
        public List<ProductImage> ProductImages { get; set; }
        public int CategoryId { get; set; }
        public Category Category { get; set; }
        [NotMapped]
        public IFormFile MainPhoto { get; set; }
        [NotMapped]
        public List<IFormFile> Photos { get; set; }
    }
}
