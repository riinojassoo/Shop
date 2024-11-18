using Shop.Core.Dto.Cocktails;

namespace Shop.Core.ServiceInterface
{
    public interface ICocktailsServices
    {
        Task<List<CocktailsDto>> CocktailsResult();
    }
}
