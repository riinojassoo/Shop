using Nancy.Json;
using Shop.Core.Dto.ChuckNorrisDtos;
using Shop.Core.Dto.FreeGamesDtos;
using Shop.Core.ServiceInterface;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace Shop.ApplicationServices.Services
{
    public class FreeGamesServices : IFreeGamesServices
    {

		public async Task<FreeGamesRootDto> FreeGamesResult(FreeGamesRootDto dto)
		{
			string url = $"https://www.freetogame.com/api/games";

			using (WebClient client = new WebClient())
			{
				string json = client.DownloadString(url);

				FreeGamesRootDto gamesResult = new JavaScriptSerializer().Deserialize<FreeGamesRootDto>(json);

				dto.id = gamesResult.id;
				dto.title = gamesResult.title;
				dto.thumbnail = gamesResult.thumbnail;
				dto.short_description = gamesResult.short_description;
				dto.game_url = gamesResult.game_url;
				dto.genre = gamesResult.genre;
				dto.platform = gamesResult.platform;
				dto.publisher = gamesResult.publisher;
				dto.developer = gamesResult.developer;
				dto.release_date = gamesResult.release_date;
				dto.freetogame_profile_url = gamesResult.freetogame_profile_url;
			}

			return dto;
		}
	}
}
