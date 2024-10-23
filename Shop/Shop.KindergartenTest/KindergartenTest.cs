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

		[Fact]
		public async Task ShouldNot_UpdateKindergarten_WhenDeletedKindergarten()
		{
			//Arrange
			KindergartenDto kindergarten = MockKindergartenData(); //loomise andmed
			KindergartenDto kindergartenUpdate = MockKindergartenData2(); //updatemise andmed

			var createdKindergarten = await Svc<IKindergartenServices>().Create(kindergarten); //loon uue kindergarteni

			//Act
			var deletedKindergarten = await Svc<IKindergartenServices>().Delete((Guid)createdKindergarten.Id); //kustutan loodud kindergarteni
			var updateKindergarten = await Svc<IKindergartenServices>().Update(kindergartenUpdate); //update-in loodud kindergartenit
			
			//Assert
			Assert.NotEqual(deletedKindergarten.Id, kindergartenUpdate.Id); //eeldan et kustutatu ja uuendatu id ei saa olla sama
		}

		[Fact]
		public async Task Should_AllowOver250CharacterAsGroupName_WhenCreated()
		{
			//Arrange
			KindergartenDto dto = new();

			dto.GroupName = new string('a', 251);
			dto.ChildrenCount = 100;
			dto.KindergartenName = "LapsedLapsed";
			dto.Teacher = "Tiiu Tamm";
			dto.CreatedAt = DateTime.Now;
			dto.UpdatedAt = DateTime.Now;

			//Act
			var result = await Svc<IKindergartenServices>().Create(dto);

			//Assert
			Assert.NotNull(result);
			Assert.Equal(251, result.GroupName.Length); //igaks juhuks et kontrollida kas ikka tuli 251 tähte
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

		private KindergartenDto MockKindergartenData2()
		{
			KindergartenDto kindergarten = new()
			{
				GroupName = "Rohutirtsud",
				ChildrenCount = 3,
				KindergartenName = "LapsedLapsedLapsed",
				Teacher = "Tiina Tamm",
				CreatedAt = DateTime.Now,
				UpdatedAt = DateTime.Now
			};

			return kindergarten;
		}
	}
}
