using Shop.Core.Dto.FreeGamesDtos;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Shop.Core.ServiceInterface
{
	public interface IFreeGamesServices
	{
		Task<FreeGamesRootDto> FreeGamesResult(FreeGamesRootDto dto);
	}
}
