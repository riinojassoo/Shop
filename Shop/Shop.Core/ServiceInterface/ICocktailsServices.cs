using Shop.Core.Dto.CocktailsDto;

namespace Shop.Core.ServiceInterface
{
    public interface ICocktailsServices
    {
        Task<List<CocktailsDto>> CocktailsResult(string cocktailName);
    }
}
