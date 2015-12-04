using System.Net.Mail;
using System.Threading.Tasks;

namespace Lisa.Bulder.WebApi
{
    public class SmtpEmailService : IEmailService
    {
        public async Task Send(string to, string subject, string body)
        {
            MailMessage message = new MailMessage();
            message.From = new MailAddress("noreply@bulder.com");
            message.To.Add(to);
            message.Subject = subject;
            message.Body = body;

            SmtpClient smtp = new SmtpClient("localhost");
            smtp.UseDefaultCredentials = true;
            await smtp.SendMailAsync(message);
        }
    }
}