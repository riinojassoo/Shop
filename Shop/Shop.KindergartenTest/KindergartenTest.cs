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
	}
}
