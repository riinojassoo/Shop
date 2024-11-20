using Shop.Core.Dto.Cocktails;
using Shop.Core.ServiceInterface;
using System.Net;
using System.Text.Json;

namespace Shop.ApplicationServices.Services
{
	public class CocktailsServices : ICocktailsServices
	{

		public async Task<List<CocktailsDto>> CocktailsResult(string CocktailName)
		{
			string url = $"https://www.thecocktaildb.com/api.php?s={CocktailName}";
			List<CocktailsDto> cocktailsList = new List<CocktailsDto>();

			using (WebClient client = new WebClient())
			{
				string json = client.DownloadString(url);

				var cocktailsResult = JsonSerializer.Deserialize<List<CocktailsDto>>(json);

				if (cocktailsResult != null)
				{
					cocktailsList.AddRange(cocktailsResult);
				}

				return cocktailsList;
			}
		}
        // New method to get cocktail by ID
        public async Task<CocktailsDto> GetCocktailById(string id)
        {
            string url = $"https://www.thecocktaildb.com/api.php?i={id}";
            using (WebClient client = new WebClient())
            {
                string json = client.DownloadString(url);
                var cocktailResult = JsonSerializer.Deserialize<List<CocktailsDto>>(json);

                if (cocktailResult != null && cocktailResult.Count > 0)
                {
                    return cocktailResult[0]; // Return the first cocktail in the result
                }
                return null;
            }
        }
    }
}