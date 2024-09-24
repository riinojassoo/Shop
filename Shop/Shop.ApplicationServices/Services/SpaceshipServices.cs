using Microsoft.EntityFrameworkCore;
using Shop.Core.Domain;
using Shop.Core.ServiceInterface;
using Shop.Data;

namespace Shop.ApplicationServices.Services
{
    public class SpaceshipServices : ISpaceshipServices
    {
		private readonly ShopContext _context;

		public SpaceshipServices
			(
			ShopContext context
			)
		{
			_context = context;
		}

		public async Task<Spaceship> DetailAsync(Guid id)
		{
			var result = await _context.Spaceships
				.FirstOrDefaultAsync( x => x.Id == id );

			return result;
		}
	}
}
