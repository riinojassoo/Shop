using Nancy.Json;
using Shop.Core.Dto.FreeGamesDtos;
using Shop.Core.ServiceInterface;

using System.Net;
using System.Text.Json;


namespace Shop.ApplicationServices.Services
{
	public class FreeGamesServices : IFreeGamesServices
	{

		public async Task<List<FreeGamesRootDto>> FreeGamesResult()
		{
			string url = $"https://www.freetogame.com/api/games";
				List<FreeGamesRootDto> gamesList = new List<FreeGamesRootDto>();

			using (WebClient client = new WebClient())
			{
				string json = client.DownloadString(url);

				//		List<FreeGamesRootDto> gamesResult = JsonSerializer.Deserialize<List<FreeGamesRootDto>>(json);

				//		foreach (var gameResult in gamesResult)
				//		{

				//			dto.id = gamesResult[0].id;
				//			dto.title = gamesResult[0].title;
				//			dto.thumbnail = gamesResult[0].thumbnail;
				//			dto.short_description = gamesResult[0].short_description;
				//			dto.game_url = gamesResult[0].game_url;
				//			dto.genre = gamesResult[0].genre;
				//			dto.platform = gamesResult[0].platform;
				//			dto.publisher = gamesResult[0].publisher;
				//			dto.developer = gamesResult[0].developer;
				//			dto.release_date = gamesResult[0].release_date;
				//			dto.freetogame_profile_url = gamesResult[0].freetogame_profile_url;
				//		}
				//	}

				//	return dto;
				//}
				var gamesResult = JsonSerializer.Deserialize<List<FreeGamesRootDto>>(json);

				if (gamesResult != null)
				{
					gamesList.AddRange(gamesResult);
				}

				return gamesResult;
			}
		}
	}
}