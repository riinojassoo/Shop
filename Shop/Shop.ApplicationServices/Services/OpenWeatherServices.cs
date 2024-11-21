using Nancy.Json;
using Shop.Core.Dto.WeatherDtos.OpenWeatherDto;
using Shop.Core.ServiceInterface;
using System.Net;

namespace Shop.ApplicationServices.Services
{
    public class OpenWeatherServices : IOpenWeatherServices
    {
        public async Task<OpenWeatherRootDto> OpenWeatherResult(OpenWeatherRootDto dto)
        {
            string ApiKey = "46003cce8d84df4ed907b62a1efce923";
            string url = $"https://api.openweathermap.org/data/2.5/weather?q={dto.Name}&appid={ApiKey}&units=metric";

            using (WebClient client = new WebClient())
            {
                string json = client.DownloadString(url);

                OpenWeatherRootDto openWeatherRootDto = new JavaScriptSerializer().Deserialize<OpenWeatherRootDto>(json);

                dto.Clouds = openWeatherRootDto.Clouds;
                dto.Coord = openWeatherRootDto.Coord;
                dto.Main = openWeatherRootDto.Main;
                dto.Rain = openWeatherRootDto.Rain;
                dto.Sys = openWeatherRootDto.Sys;
                dto.Weather = openWeatherRootDto.Weather;
                dto.Wind = openWeatherRootDto.Wind;
                dto.Base = openWeatherRootDto.Base;
                dto.Visibility = openWeatherRootDto.Visibility;
                dto.Dt = openWeatherRootDto.Dt;
                dto.Timezone = openWeatherRootDto.Timezone;
                dto.Id = openWeatherRootDto.Id;
                dto.Cod = openWeatherRootDto.Cod;

                return openWeatherRootDto;
            }
        }
    }
}
