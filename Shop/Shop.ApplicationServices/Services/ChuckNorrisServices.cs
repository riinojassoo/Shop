using Nancy.Json;
using Shop.Core.Dto.ChuckNorrisDtos;
using Shop.Core.ServiceInterface;
using System.Net;

namespace Shop.ApplicationServices.Services
{
    public class ChuckNorrisServices : IChuckNorrisServices
    {
		public async Task<ChuckNorrisRootDto> ChuckNorrisResult(ChuckNorrisRootDto dto)
		{
			string url = $"https://api.chucknorris.io/jokes/random";

			using (WebClient client = new WebClient())
			{
				string json = client.DownloadString(url);

				ChuckNorrisRootDto chuckResult = new JavaScriptSerializer().Deserialize<ChuckNorrisRootDto>(json);

				dto.Id = chuckResult.Id;
				dto.Categories = chuckResult.Categories;
				dto.CreatedAt = chuckResult.CreatedAt;
				dto.IconUrl = chuckResult.IconUrl;
				dto.UpdatedAt = chuckResult.UpdatedAt;
				dto.Value = chuckResult.Value;
			}

			return dto;
		}
    }
}
