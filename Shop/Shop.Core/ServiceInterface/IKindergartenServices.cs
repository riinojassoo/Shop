using Shop.Core.Domain;

namespace Shop.Core.ServiceInterface
{
    public interface IKindergartenServices
    {
        Task<Kindergarten> DetailAsync(Guid id);
    }
}
