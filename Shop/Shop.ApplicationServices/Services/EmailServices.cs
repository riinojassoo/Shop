﻿using Microsoft.Extensions.Configuration;
using MimeKit;
using Shop.Core.Dto.Emails;
using Shop.Core.ServiceInterface;
using MailKit.Net.Smtp;


namespace Shop.ApplicationServices.Services
{
	public class EmailServices : IEmailServices
	{
		private readonly IConfiguration _config;

		public EmailServices
			(
			IConfiguration config
			)
		{
			_config = config;
		}

		public void SendEmail(EmailDto dto)
		{
			var email = new MimeMessage();
			email.From.Add(MailboxAddress.Parse(_config.GetSection("EmailUserName").Value));
			email.To.Add(MailboxAddress.Parse(dto.To));
			email.Subject = dto.Subject;
            //email.Body = new TextPart(MimeKit.Text.TextFormat.Html)
            //{
            //	Text = dto.Body
            //};

            var builder = new BodyBuilder
            {
                HtmlBody = dto.Body
            };

            // Add attachments if they exist
            if (dto.Attachments != null && dto.Attachments.Any())
            {
                foreach (var attachment in dto.Attachments)
                {
                    if (attachment.Length > 0)
                    {
                        using var memoryStream = new MemoryStream();
						attachment.CopyTo(memoryStream);
                        builder.Attachments.Add(attachment.FileName, memoryStream.ToArray());
                    }
                }
            }

            email.Body = builder.ToMessageBody();

            //kindlasti kasutada mailkit.net.smtp
            using var smtp = new SmtpClient();

			//siin tuleb valida õige port ja kasutada securesocket optionit
			//autentida
			//saada email
			//vabasta ressurss
			smtp.Connect(_config.GetSection("EmailHost").Value, 587, MailKit.Security.SecureSocketOptions.StartTls);

			smtp.Authenticate(_config.GetSection("EmailUsername").Value, _config.GetSection("EmailPassword").Value);

			smtp.Send(email);

			smtp.Disconnect(true);
		}
	}
}