using Microsoft.AspNetCore.Mvc;

namespace MultiShopProject.Areas.MultiAdmin.Controllers
{
    public class DashBoardController : Controller
    {
        [Area("MultiAdmin")]

        public IActionResult Index()
        {
            return View();
        }
    }
}
