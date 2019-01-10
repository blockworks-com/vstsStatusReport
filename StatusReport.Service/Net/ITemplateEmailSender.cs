using System.Threading.Tasks;
using System.Net.Mail;

namespace AxSoft.TemplateEmail.Net
{
	public interface ITemplateEmailSender
	{
		string LayoutFilePath { get; set; }

		void Send(string templatePath, object variables, string to, string cc, string subject);

		Task SendAsync(string templatePath, object variables, string to, string cc, string subject);

        MailMessage ConstructMailMessage(string templatePath, object variables, string to, string cc, string subject);
    }
}