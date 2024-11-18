using Nancy.Json;
using Shop.Core.Dto.Cocktails;
using Shop.Core.Dto.WeatherDtos.AccuWeatherDtos;
using Shop.Core.ServiceInterface;
using System.Collections.Generic;
using System.Net;

namespace Shop.ApplicationServices.Services
{
    public class CocktailsServices : ICocktailsServices
    {
        public async Task<List<CocktailsDto>> CocktailsResult()
        {
            string url = $"www.thecocktaildb.com/api/json/v1/1/search.php?s={dto.strDrink}";
            List<CocktailsDto> cocktailsList = new List<CocktailsDto>();

            using (WebClient client = new WebClient())
            {
                string json = client.DownloadString(url);

                List<CocktailsDto> coctailsResult = new JavaScriptSerializer().Deserialize<List<CocktailsDto>>(json);

                dto.strDrink = coctailsResult[0].strDrink;
            }

    }
}


