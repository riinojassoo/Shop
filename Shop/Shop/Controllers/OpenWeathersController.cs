using Microsoft.AspNetCore.Mvc;
using Shop.Core.Dto.WeatherDtos.OpenWeatherDto;
using Shop.Core.ServiceInterface;
using Shop.Models.OpenWeathers;

namespace Shop.Controllers
{
    public class OpenWeathersController : Controller
    {
        private readonly IOpenWeatherServices _openWeatherServices;

        public OpenWeathersController
            (
            IOpenWeatherServices openWeatherServices
            )
        {
            _openWeatherServices = openWeatherServices;
        }
        public IActionResult Index()
        {
            return View();
        }

        [HttpPost]
        public IActionResult SearchName(OpenWeathersIndexViewModel model)
        {
            if (ModelState.IsValid)
            {
                return RedirectToAction("Name", new { name = model.Name });
            }

            return View("Index", model);
        }

        [HttpGet]
        public async Task<IActionResult> Name(string name)
        {
            OpenWeatherRootDto dto = new();
            dto.Name = name;

            await _openWeatherServices.OpenWeatherResult(dto);
            OpenWeathersSearchViewModel vm = new();

            vm.Name = dto.Name;
            vm.Temp = dto.Main.Temp;
            vm.Feels_like = dto.Main.Feels_like;
            vm.Humidity = dto.Main.Humidity;
            vm.Pressure = dto.Main.Pressure;
            vm.Speed = dto.Wind.Speed;

            return View("Name", vm);
        }
    }
}
