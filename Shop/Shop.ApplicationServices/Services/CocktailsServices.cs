using Shop.Core.Dto.CocktailsDto;
using Shop.Core.ServiceInterface;
using System.Net;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace Shop.ApplicationServices.Services
{
	public class CocktailsServices : ICocktailsServices
	{

		public async Task<List<CocktailsDto>> CocktailsResult(string cocktailName)
		{
			string url = $"https://www.thecocktaildb.com/api/json/v1/1/search.php?s={cocktailName}";
			List<CocktailsDto> cocktailsList = new List<CocktailsDto>();

			using (WebClient client = new WebClient())
			{
				string json = client.DownloadString(url);

				var apiResult = JsonSerializer.Deserialize<CocktailsApiResult>(json);

				if (apiResult?.Drinks != null)
				{
					cocktailsList.AddRange(apiResult.Drinks);
				}

				return cocktailsList;
			}
		}
		public class CocktailsApiResult
		{
			[JsonPropertyName("drinks")]
			public List<CocktailsDto> Drinks { get; set; }
		}
	}
}