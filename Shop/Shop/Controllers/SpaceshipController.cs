using Microsoft.AspNetCore.Mvc;

namespace Shop.Controllers
{
    public class SpaceshipController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
