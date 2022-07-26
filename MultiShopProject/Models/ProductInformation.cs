using MultiShopProject.Models.Base;
using System.Collections.Generic;

namespace MultiShopProject.Models
{
    public class ProductInformation:BaseEntity
    {
        public string Name { get; set; }

        public string Information { get; set; }


        public List<Product> Products { get; set; }
    }
}
