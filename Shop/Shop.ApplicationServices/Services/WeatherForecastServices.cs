using Nancy.Json;
using Shop.Core.Dto.WeatherDtos;
using Shop.Core.Dto.WeatherDtos.AccuWeatherDtos;
using Shop.Core.ServiceInterface;
using System.Net;

namespace Shop.ApplicationServices.Services
{
	public class WeatherForecastServices
	{
		public async Task<AccuLocationWeatherResultDto> AccuLocationWeatherResult(AccuLocationWeatherResultDto dto)
		{
			string accuApiKey = "uW2QjVRLeDDSOM34wUFRl1nBch0MzXG5";
			string url = $"http://dataservice.accuweather.com/locations/v1/cities/search?apikey={accuApiKey}&q={dto.CityName}";

			using (WebClient client = new WebClient())
			{
				string json = client.DownloadString(url);

				AccuLocationRootDto accuResult = new JavaScriptSerializer().Deserialize<AccuLocationRootDto>(json);
			}

			return dto;
		}
	}
}
