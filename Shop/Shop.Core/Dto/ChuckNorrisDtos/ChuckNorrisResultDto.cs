using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Shop.Core.Dto.ChuckNorrisDtos
{
	public class ChuckNorrisResultDto
	{
		public string Category { get; set; }
		public DateTime? CreatedAt { get; set; }
		public DateTime? UpdatedAt { get; set; }
		public Guid? Id { get; set; }
		public string Value { get; set; }
	}
}
