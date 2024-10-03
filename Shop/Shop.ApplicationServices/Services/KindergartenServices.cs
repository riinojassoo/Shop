﻿using Microsoft.EntityFrameworkCore;
using Shop.Core.Domain;
using Shop.Core.ServiceInterface;
using Shop.Data;

namespace Shop.ApplicationServices.Services
{
    public class KindergartenServices : IKindergartenServices
    {
		private readonly ShopContext _context;

		public KindergartenServices
			(
				ShopContext context
			)
		{
			_context = context;
		}

		public async Task<Kindergarten> DetailAsync(Guid id)
		{
			var result = await _context.Kindergartens
				.FirstOrDefaultAsync(x => x.Id == id);

			return result;
		}
	}
}
