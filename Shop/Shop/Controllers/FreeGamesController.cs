using Microsoft.AspNetCore.Mvc;
using Shop.ApplicationServices.Services;
using Shop.Core.Dto.FreeGamesDtos;
using Shop.Core.ServiceInterface;
using Shop.Models.FreeGames;

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

		[HttpPost]
		public IActionResult GenerateList()
		{
			return RedirectToAction("GenerateGamesList");
		}

		[HttpGet]
		public async Task<IActionResult> GenerateGamesList()
		{

			FreeGamesRootDto dto = new FreeGamesRootDto();

			await _freeGamesServices.FreeGamesResult(dto);

			if (string.IsNullOrEmpty(dto.short_description))
			{
				ModelState.AddModelError(string.Empty, "List not found.");
				return View("Error");
			}
			var vm = new FreeGamesIndexViewModel
			{
				id = dto.id,
				title = dto.title,
				genre = dto.genre,
				platform = dto.platform,
				short_description = dto.short_description,
			};

			return View("Index", vm);

		}
	}
}
