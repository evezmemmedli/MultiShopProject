using MultiShopProject.Models.Base;
using System.Collections.Generic;

namespace MultiShopProject.Models
{
    public class Product : BaseEntity
    {
        public string Name { get; set; }

        public decimal Price { get; set; }

        public string Desc { get; set; }

        public int Quantity { get; set; }

        public ProductInformation ProductInformation { get; set; }
        public List<ProductImage> ProductImages { get; set; }
        public int CategoryId { get; set; }
        public Category Category { get; set; }


    }
}
