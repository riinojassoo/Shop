using Shop.Core.Dto;
using Shop.Core.ServiceInterface;
using System;


namespace ShopRealEstateTest
{
    public class RealEstateTest : TestBase
    {
        [Fact]
        public async Task ShouldNot_AddEmptyRealEstate_WhenReturnResult()
        {
            //Arrange
            RealEstateDto dto = new();

            dto.Size = 100;
            dto.Location = "asd";
            dto.RoomNumber = 1;
            dto.BuildingType = "asd";
            dto.CreatedAt = DateTime.Now;
            dto.ModifiedAt = DateTime.Now;

            //Act
            var result = await Svc<IRealEstateServices>().Create(dto);

            //Assert
            Assert.NotNull(result);
        }

        [Fact]
        public async Task ShouldNot_GetByIdRealEstate_WhenReturnsNotEqual()
        {
            //Arrange
            Guid wrongGuid = Guid.Parse(Guid.NewGuid().ToString());
            Guid guid = Guid.Parse("6e07475d-9ca5-4449-96cd-3bbb9de5bc81");

            //Act
            await Svc<IRealEstateServices>().GetAsync(guid);

            //Assert
            Assert.NotEqual(wrongGuid, guid);

		}

        [Fact]
        public async Task Should_GetByIdRealEstate_WhenReturnsEqual()
        {
            Guid databaseGuid = Guid.Parse("89c057b7-918c-4296-992a-0938a16e3d1d");
            Guid guid = Guid.Parse("89c057b7-918c-4296-992a-0938a16e3d1d");

			await Svc<IRealEstateServices>().GetAsync(guid);

            Assert.Equal(databaseGuid, guid);
		}

        [Fact]
        public async Task should_DeleteByIdRealEstate_WhenDeleteRealEstate()
        {
            //Arrange
            RealEstateDto realEstate = MockRealEstateData();

			//mängult sisestan andmed, seejärel kustutan ära
			var addRealEstate = await Svc<IRealEstateServices>().Create(realEstate);

			//Act
			var result = await Svc<IRealEstateServices>().Delete((Guid)addRealEstate.Id);

            //Assert
            Assert.Equal(result, addRealEstate);
		}

        [Fact]
        public async Task shouldNot_DeleteByIdRealEstate_WhenDidNotDeleteRealEstate()
        {
            //Arrange
            RealEstateDto realEstate = MockRealEstateData();

            var realEstate1 = await Svc<IRealEstateServices>().Create(realEstate);
            var realEstate2 = await Svc<IRealEstateServices>().Create(realEstate);

			//Act
			var result = await Svc<IRealEstateServices>().Delete((Guid)realEstate2.Id);

            //Assert
            Assert.NotEqual(result.Id, realEstate1.Id);

		}

        [Fact]
        public async Task Should_UpdateRealEstate_WhenUpdateData()
        {
            var guid = new Guid("89c057b7-918c-4296-992a-0938a16e3d1d");

            RealEstateDto dto = MockRealEstateData();

            RealEstateDto domain = new();

            domain.Id = Guid.Parse("89c057b7-918c-4296-992a-0938a16e3d1d");
            domain.Size = 99;
            domain.Location = "qwe";
            domain.RoomNumber = 456;
            domain.BuildingType = "qwe";
            domain.CreatedAt = DateTime.UtcNow;
            domain.ModifiedAt = DateTime.UtcNow;

            await Svc<IRealEstateServices>().Update(dto);

            Assert.Equal(guid, domain.Id);
            Assert.DoesNotMatch(dto.Location, domain.Location);
            Assert.DoesNotMatch(dto.RoomNumber.ToString(), domain.RoomNumber.ToString());
            Assert.NotEqual(dto.Size, domain.Size);
        }

        [Fact]
        public async Task Should_UpdateRealEstate_WhenUpdateDataVersion2()
        {
			//kasutame kahte mock andmebaasi
			//ja ss võrdleme neid omavahel
			RealEstateDto dto = MockRealEstateData();
            var createRealEstate = await Svc<IRealEstateServices>().Create(dto);
            //RealEstateDto domain = MockRealEstateData2();

			//await Svc<IRealEstateServices>().Update(dto);

			//Assert.Equal(dto.Id, domain.Id);
			//Assert.DoesNotMatch(dto.Location, domain.Location);
			//Assert.DoesNotMatch(dto.RoomNumber.ToString(), domain.RoomNumber.ToString());
			//Assert.NotEqual(dto.Size, domain.Size);

            RealEstateDto update = MockRealEstateData2();
            var result = await Svc<IRealEstateServices>().Update(update);

            Assert.DoesNotMatch(result.Location, createRealEstate.Location);
            Assert.NotEqual(result.ModifiedAt, createRealEstate.ModifiedAt);

		}

        [Fact]
        public async Task ShouldNot_UpdateRealEstate_WhenDidNotUpdateData()
        {
            RealEstateDto dto = MockRealEstateData();
            var createdRealEstate = await Svc<IRealEstateServices>().Create(dto);//vajutab save

            RealEstateDto nullUpdate = MockNullRealEstateData();
            var result = await Svc<IRealEstateServices>().Update(nullUpdate);//uued andmed mis peale tulevaf

            Assert.NotEqual(createdRealEstate.Id, result.Id);
		}

        [Fact]
        public async Task ShouldNot_UploadRealEstate_xslxFiles()
        {
			RealEstateDto dto = MockRealEstateData();

			// Create a mock .xlsx file (as a MemoryStream or file object)
			var xlsxFile = new MemoryStream();
			var writer = new StreamWriter(xlsxFile);
			writer.Write("Mock xlsx content");
			writer.Flush();
			xlsxFile.Position = 0; // Reset the stream position

			
		}


		private RealEstateDto MockRealEstateData()
        {
            RealEstateDto realEstate = new()
            {
                Size = 100,
                Location = "asd",
                RoomNumber = 1,
                BuildingType = "asd",
                CreatedAt = DateTime.Now,
                ModifiedAt = DateTime.Now,
            };

            return realEstate;
        }

		private RealEstateDto MockRealEstateData2()
		{
			RealEstateDto realEstate = new()
			{
				Size = 99,
				Location = "qwe",
				RoomNumber = 2,
				BuildingType = "qwe",
				CreatedAt = DateTime.Now.AddYears(1),
				ModifiedAt = DateTime.Now.AddYears(1),
			};

			return realEstate;
		}

		private RealEstateDto MockNullRealEstateData()
		{
			RealEstateDto realEstate = new()
			{
                Id = null,
				Size = 99,
				Location = "qwe",
				RoomNumber = 2,
				BuildingType = "qwe",
				CreatedAt = DateTime.Now.AddYears(-1),
				ModifiedAt = DateTime.Now.AddYears(-1),
			};

			return realEstate;
		}
	}
}