using SendGrid;
using System.Net;
using System.Net.Mail;
using System.Threading.Tasks;

namespace Lisa.Bulder.WebApi
{
    public class SendGridEmailService : IEmailService
    {
        public async Task Send(string to, string subject, string body)
        {
            SendGridMessage message = new SendGridMessage();
            message.From = new MailAddress("noreply@bulder.com");
            message.AddTo(to);
            message.Subject = subject;
            message.Text = body;

            var credentials = new NetworkCredential("azure_0d222e73bb333249b0e60eb91c9ace1f@azure.com", "qDoHuykdvN5&D@uamz^zyMlaAYq2&6BA9s$EJjhx3jutJ9r3W");
            var transportWeb = new Web(credentials);
            await transportWeb.DeliverAsync(message);
        }
    }
}
