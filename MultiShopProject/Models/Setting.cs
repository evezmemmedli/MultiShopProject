using MultiShopProject.Models.Base;
using System.Collections.Generic;

namespace MultiShopProject.Models
{
    public class Setting:BaseEntity
    {
        List<Category>Categories { get; set; }
    }
}
