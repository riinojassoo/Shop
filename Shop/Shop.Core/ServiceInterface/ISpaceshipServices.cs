using Shop.Core.Domain;

namespace Shop.Core.ServiceInterface
{
	public class ISpaceshipServices
	{
		Task<Spaceship> DetailAsync(Guid id);
	}
}
