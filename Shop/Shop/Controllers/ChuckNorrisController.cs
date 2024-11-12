using Microsoft.AspNetCore.Mvc;
using Shop.Core.Dto.ChuckNorrisDtos;
using Shop.Core.ServiceInterface;
using Shop.Models.ChuckNorris;

namespace Shop.Controllers
{
	public class ChuckNorrisController : Controller
	{
		private readonly IChuckNorrisServices _chuckNorrisServices;

		public ChuckNorrisController(IChuckNorrisServices chuckNorrisServices)
		{
			_chuckNorrisServices = chuckNorrisServices;
		}
		public IActionResult Index()
		{
			return View();
		}

		[HttpPost]
		public IActionResult GenerateJoke()
		{
			return RedirectToAction("ChuckNorrisJoke");
		}

		[HttpGet]
        public async Task<IActionResult> ChuckNorrisJoke()
        {
			ChuckNorrisRootDto dto = new ChuckNorrisRootDto();

			await _chuckNorrisServices.ChuckNorrisResult(dto);

			if (string.IsNullOrEmpty(dto.Value))
			{
				ModelState.AddModelError(string.Empty, "Joke not found.");
				return View("Error");
			}

			var vm = new ChuckNorrisJokeGeneratorViewModel
			{
				CreatedAt = dto.CreatedAt,
				IconUrl = dto.IconUrl,
				Id = dto.Id,
				UpdatedAt = dto.UpdatedAt,
				Url = dto.Url,
				Value = dto.Value,
			};

			return View("JokeGenerator", vm);
		}
	}
}
