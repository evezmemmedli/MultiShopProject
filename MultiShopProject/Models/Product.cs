using Microsoft.AspNetCore.Http;
using MultiShopProject.Models.Base;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;

namespace MultiShopProject.Models
{
    public class Product : BaseEntity
    {

        public string Name { get; set; }

        public decimal Price { get; set; }

        public string Desc { get; set; }

        public int Quantity { get; set; }

        public int ProductInformationId { get; set; }
        public  ProductInformation ProductInformation { get; set; }
        public  List<ProductImage> ProductImages { get; set; }
        public int CategoryId { get; set; }
        public  Category Category { get; set; }

        [NotMapped]
        public IFormFile MainPhoto { get; set; }

        [NotMapped]
        public List<IFormFile> Photos { get; set; }


    }
}
