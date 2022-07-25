using MultiShopProject.Models.Base;
using System.Collections.Generic;

namespace MultiShopProject.Models
{
    public class ProductInformation:BaseEntity
    {
        public string Description { get; set; }

        public string Information { get; set; }

        public string Review { get; set; }

        public List<Product> Products { get; set; }
    }
}
