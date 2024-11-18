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
        public async Task<IActionResult> Index()
        {
			var dtoList = await _freeGamesServices.FreeGamesResult();

			if (dtoList == null || !dtoList.Any())
			{
				ModelState.AddModelError(string.Empty, "No games found.");
				return View("Error");
			}

			// Map each DTO to a ViewModel
			var vmList = dtoList.Select(dto => new FreeGamesIndexViewModel
			{
				id = dto.id,
				title = dto.title,
				genre = dto.genre,
				platform = dto.platform,
				short_description = dto.short_description
			}).ToList();

			// Pass the list of ViewModels to the Index view
			return View(vmList);
            
        }
	}
}
