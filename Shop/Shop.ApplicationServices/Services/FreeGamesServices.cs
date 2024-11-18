using Nancy.Json;
using Shop.Core.Dto.FreeGamesDtos;
using Shop.Core.ServiceInterface;

using System.Net;
using System.Text.Json;


namespace Shop.ApplicationServices.Services
{
	public class FreeGamesServices : IFreeGamesServices
	{

		public async Task<List<FreeGamesRootDto>> FreeGamesResult(string category = null)
		{
			string url = $"https://www.freetogame.com/api/games";
				List<FreeGamesRootDto> gamesList = new List<FreeGamesRootDto>();

			using (WebClient client = new WebClient())
			{
				string json = client.DownloadString(url);

				var gamesResult = JsonSerializer.Deserialize<List<FreeGamesRootDto>>(json);

				if (gamesResult != null)
				{
					gamesList.AddRange(gamesResult);
				}

				return gamesResult ?? new List<FreeGamesRootDto>();
			}
		}
	}
}