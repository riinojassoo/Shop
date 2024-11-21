using Shop.Core.Dto.WeatherDtos.OpenWeatherDto;

namespace Shop.Core.ServiceInterface
{
    public interface IOpenWeatherServices
    {
        Task<OpenWeatherRootDto> OpenWeatherResult(OpenWeatherRootDto dto);
    }
}
