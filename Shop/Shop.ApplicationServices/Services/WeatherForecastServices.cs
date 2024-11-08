using Nancy.Json;
using Shop.Core.Dto.WeatherDtos.AccuWeatherDtos;
using Shop.Core.ServiceInterface;
using System.Net;

namespace Shop.ApplicationServices.Services
{
    public class WeatherForecastServices : IWeatherForecastServices
	{
		public async Task<AccuLocationWeatherResultDto> AccuWeatherResult(AccuLocationWeatherResultDto dto)
		{
			string accuApiKey = "uW2QjVRLeDDSOM34wUFRl1nBch0MzXG5";
			string url = $"http://dataservice.accuweather.com/locations/v1/cities/search?apikey={accuApiKey}&q={dto.CityName}";
			//127964

			//sordib välja City koodi et saaks ilma vaadata - tulemus linna kood
			using (WebClient client = new WebClient())
			{
				string json = client.DownloadString(url);

				List<AccuLocationRootDto> accuResult = new JavaScriptSerializer().Deserialize<List<AccuLocationRootDto>>(json);

				dto.CityName = accuResult[0].LocalizedName;
				dto.CityCode = accuResult[0].Key;
			}

            string urlWeather = $"http://dataservice.accuweather.com/forecasts/v1/daily/1day/{dto.CityCode}?apikey={accuApiKey}&metric=true";

			//City koodi järgi ilmateade - tulemus ilmateade
			using (WebClient client = new WebClient())
			{
				string json = client.DownloadString(urlWeather);

				AccuWeatherRootDto weatherRootDto = new JavaScriptSerializer().Deserialize<AccuWeatherRootDto>(json);

				dto.EffectiveDate = weatherRootDto.Headline.EffectiveDate.ToString("yyyy-MM-dd");
				dto.EffectiveEpochDate = weatherRootDto.Headline.EffectiveEpochDate;
				dto.Severity = weatherRootDto.Headline.Severity;
				dto.Text = weatherRootDto.Headline.Text;
				dto.Category = weatherRootDto.Headline.Category;
				dto.EndDate = weatherRootDto.Headline.EndDate.ToString("yyyy-MM-dd");
				dto.EndEpochDate = weatherRootDto.Headline.EndEpochDate;

				dto.MobileLink = weatherRootDto.Headline.MobileLink;
				dto.Link = weatherRootDto.Headline.Link;

				var forecast = weatherRootDto.DailyForecasts[0];

				dto.DailyForecastsDate = forecast.Date.ToString("yyyy-MM-dd");
				dto.DailyForecastsEpochDate = forecast.EpochDate;

				dto.TempMinValue = forecast.Temperature.Minimum.Value;
				dto.TempMinUnit = forecast.Temperature.Minimum.Unit;
                dto.TempMinUnitType = forecast.Temperature.Minimum.UnitType;

				dto.TempMaxValue = forecast.Temperature.Maximum.Value;
				dto.TempMaxUnit = forecast.Temperature.Maximum.Unit;
				dto.TempMaxUnitType = forecast.Temperature.Maximum.UnitType;

				dto.DayIcon = forecast.Day.Icon;
				dto.DayIconPhrase = forecast.Day.IconPhrase;
				dto.DayHasPrecipitation = forecast.Day.HasPrecipitation;
				dto.DayPrecipitationType = forecast.Day.PrecipitationType;
				dto.DayPrecipitationIntensity = forecast.Day.PrecipitationIntensity;

				dto.NightIcon = forecast.Night.Icon;
				dto.NightIconPhrase = forecast.Night.IconPhrase;
				dto.NightHasPrecipitation = forecast.Night.HasPrecipitation;
				dto.NightPrecipitationType = forecast.Night.PrecipitationType;
				dto.NightPrecipitationIntensity = forecast.Night.PrecipitationIntensity;
            }

            return dto;
		}
	}
}
