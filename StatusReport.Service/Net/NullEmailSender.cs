using System;
using System.Net.Mail;
using System.Threading.Tasks;

namespace AxSoft.TemplateEmail.Net
{
	public class NullEmailSender : IEmailSender
	{
		public void Send(MailMessage message)
		{
            throw new NotImplementedException();
		}

		public Task SendAsync(MailMessage message)
		{
            throw new NotImplementedException();
        }
    }
}