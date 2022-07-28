using Microsoft.AspNetCore.Identity;

namespace MultiShopProject.Models
{
    public class AppUser:IdentityUser
    {
        public string Firstname { get; set;}

        public string Lastname { get; set;}
    }
}
