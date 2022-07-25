using Microsoft.AspNetCore.Mvc;

namespace MultiShopProject.Controllers
{
    public class OrderController : Controller
    {
        public IActionResult ShoppinCart ()
        {
            return View();
        }
        public IActionResult Cart()
        {
            return View();
        }
    }
}
