﻿namespace Shop.Core.Dto
{
	public class RealEstateDto
	{
		public Guid? Id { get; set; }
		public double Size { get; set; }
		public string Location { get; set; }
		public int RoomNumber { get; set; }
		public string BuildingType { get; set; }

		public DateTime CreatedAt { get; set; }
		public DateTime ModifiedAt { get; set; }
	}
}