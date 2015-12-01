using System.Threading.Tasks;

namespace Lisa.Bulder.WebApi
{
    public interface IEmailService
    {
        Task Send(string to, string subject, string body);
    }
}