using Shop.Core.Dto.WeatherDtos.OpenWeatherDto;

namespace Shop.Models.OpenWeathers
{
    public class OpenWeathersSearchViewModel
    {
        //Clouds
        public int All { get; set; }

        //Coord
        public double Lon { get; set; }
        public double Lat { get; set; }

        //Main
        public double Temp { get; set; }
        public double Feels_like { get; set; }
        public double Temp_min { get; set; }
        public double Temp_max { get; set; }
        public int Pressure { get; set; }
        public int Humidity { get; set; }
        public int Sea_level { get; set; }
        public int Grnd_level { get; set; }

        //Rain
        public double _1h { get; set; }

        //Root
        public Coord Coord { get; set; }
        public List<Weather> Weather { get; set; }
        public string Base { get; set; }
        public Main Main { get; set; }
        public int Visibility { get; set; }
        public Wind Wind { get; set; }
        public Rain Rain { get; set; }
        public Clouds Clouds { get; set; }
        public int Dt { get; set; }
        public Sys Sys { get; set; }
        public int Timezone { get; set; }
        public int Id { get; set; }
        public string Name { get; set; }
        public int Cod { get; set; }

        //Sys
        public int Type { get; set; }
        public string Country { get; set; }
        public int Sunrise { get; set; }
        public int Sunset { get; set; }

        //Weather
        public string Description { get; set; }
        public string Icon { get; set; }

        //Wind
        public double Speed { get; set; }
        public int Deg { get; set; }
        public double Gust { get; set; }
    }
}
