using Microsoft.AspNetCore.Mvc;

namespace Shop.Controllers
{
    public class CocktailsController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
