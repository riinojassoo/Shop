﻿using System.Text.Json.Serialization;

namespace Shop.Core.Dto.FreeGamesDtos
{
	public class FreeGamesRootDto
	{
        [JsonPropertyName("id")]
		public int id { get; set; }

		[JsonPropertyName("title")]
		public string title { get; set; }

		[JsonPropertyName("thumbnail")]
		public string thumbnail { get; set; }

		[JsonPropertyName("short_description")]
		public string short_description { get; set; }

		[JsonPropertyName("game_url")]
		public string game_url { get; set; }

		[JsonPropertyName("genre")]
		public string genre { get; set; }

		[JsonPropertyName("platform")]
		public string platform { get; set; }

		[JsonPropertyName("publisher")]
		public string publisher { get; set; }

		[JsonPropertyName("developer")]
		public string developer { get; set; }

		[JsonPropertyName("release_date")]
		public string release_date { get; set; }

		[JsonPropertyName("freetogame_profile_url")]
		public string freetogame_profile_url { get; set; }
	}
}
