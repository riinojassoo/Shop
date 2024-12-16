using Shop.Core.Dto.Emails;

namespace Shop.Core.ServiceInterface
{
    public interface IEmailServices
    {
		public void SendEmail(EmailDto dto);
		public void SendEmailToken(EmailTokenDto dto, string token);
	}
}
