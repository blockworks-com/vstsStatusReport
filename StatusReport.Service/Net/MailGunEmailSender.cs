using System;
using System.Net.Mail;
using System.Threading.Tasks;
using RestSharp;
using RestSharp.Authenticators;

namespace AxSoft.TemplateEmail.Net
{
	public class MailGunEmailSender : IEmailSender
	{
		public void Send(MailMessage message)
		{
			using (var smtpServer = new SmtpClient())
			{
                RestClient client = new RestClient();
                client.BaseUrl = new Uri("https://api.mailgun.net/v3");
                client.Authenticator =
                    new HttpBasicAuthenticator("api",
                                                "");
                RestRequest request = new RestRequest();
                request.AddParameter("domain", "", ParameterType.UrlSegment);
                //request.Resource = "{domain}/messages";
                request.AddParameter("from", "");
                request.AddParameter("to", "");
                request.AddParameter("subject", "StatusReport Tester");
                request.AddParameter("text", "Testing some Mailgun awesomeness!");
                request.Method = Method.POST;
                var result = client.Execute(request);

                return;
            }
		}

		public Task SendAsync(MailMessage message)
		{
			return Task.Factory.StartNew(() => Send(message));
		}
	}
}