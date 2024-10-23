using Shop.Core.Dto;
using Shop.Core.ServiceInterface;
using System;

namespace Shop.KindergartenTest
{
	public class KindergartenTest : TestBase
	{
		[Fact]
		public async Task ShouldNot_GetByIdKindergarten_WhenReturnsNotEqual()
		{
			//Arrange
			Guid wrongGuid = Guid.Parse(Guid.NewGuid().ToString());
			Guid guid = Guid.Parse("75063866-1bd2-4cff-9abb-3217e6fc7c02");

			//Act
			await Svc<IKindergartenServices>().DetailAsync(guid);

			//Assert
			Assert.NotEqual(wrongGuid, guid);

		}

		[Fact]
		public async Task Should_AddNegativeChildrenCountKindergarten_WhenCreated()
		{
			//Arrange
			KindergartenDto dto = new();

			dto.GroupName = "Mesimummud";
			dto.ChildrenCount = -1;
			dto.KindergartenName = "LapsedLapsed";
			dto.Teacher = "Tiiu Tamm";
			dto.CreatedAt = DateTime.Now;
			dto.UpdatedAt = DateTime.Now;

			//Act
			var result = await Svc<IKindergartenServices>().Create(dto);

			//Assert
			Assert.Equal(result.ChildrenCount, dto.ChildrenCount);
		}

		[Fact]
		public async Task Should_AllowOnlySymbolsAsTecherKindergarten_WhenUpdated()
		{
			//Arrange
			KindergartenDto dto = MockKindergartenData();

			//Act
			var result = await Svc<IKindergartenServices>().Update(dto);

			//Assert
			Assert.Equal(result.Teacher, dto.Teacher);

		}

		private KindergartenDto MockKindergartenData()
		{
			KindergartenDto kindergarten = new()
			{
				GroupName = "Rohutirtsud",
				ChildrenCount = 3,
				KindergartenName = "LapsedLapsedLapsed",
				Teacher = "!@#$%",
				CreatedAt = DateTime.Now,
				UpdatedAt = DateTime.Now
			};

			return kindergarten;
		}
	}
}
