using Microsoft.AspNetCore.Mvc;
using Shop.ApplicationServices.Services;
using Shop.Core.ServiceInterface;

namespace Shop.Controllers
{
    public class FreeGamesController : Controller
    {
        private readonly IFreeGamesServices _freeGamesServices;
        public FreeGamesController(IFreeGamesServices freeGamesServices)
        {
            _freeGamesServices = freeGamesServices;
        }
        public IActionResult Index()
        {
            return View();
        }
    }
}
