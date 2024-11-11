using Microsoft.AspNetCore.Mvc;
using Shop.Core.ServiceInterface;
using Shop.Models.ChuckNorrises;

namespace Shop.Controllers
{
	public class ChuckNorrisController : Controller
	{
		private readonly IChuckNorrisServices _chuckNorrisServices;

		public ChuckNorrisController
			(
			IChuckNorrisServices chuckNorrisServices
			)
		{
			_chuckNorrisServices = chuckNorrisServices;
		}
		public IActionResult Index()

		{
			return View();
		}
	}
}
