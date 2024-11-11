using Nancy.Json;
using Shop.Core.Dto.ChuckNorrisDtos;
using Shop.Core.Dto.WeatherDtos.AccuWeatherDtos;
using Shop.Core.ServiceInterface;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace Shop.ApplicationServices.Services
{
    public class ChuckNorrisServices : IChuckNorrisServices
    {
		public async Task<ChuckNorrisResultDto> ChuckNorrisResult(ChuckNorrisResultDto dto)
		{
			string url = "https://api.chucknorris.io/jokes/random";

			using (WebClient client = new WebClient())
			{
				string json = client.DownloadString(url);
			}

			return dto;
		}
    }
}
