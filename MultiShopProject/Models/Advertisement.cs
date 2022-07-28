using Microsoft.AspNetCore.Http;
using MultiShopProject.Models.Base;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace MultiShopProject.Models
{
    public class Advertisement:BaseEntity
    {
        public string Image { get; set; }
        [Required]
        public string Title { get; set; }
        [Required]
        public string Discount { get; set; }
        [Required]
        public string ButtonUrl { get; set; }

        [Required]
        public byte Order { get; set; }

        [NotMapped]
        public IFormFile Photo { get; set; }
    }
}
