using Microsoft.AspNet.Mvc;
using System.Net.Mail;
using System.Threading.Tasks;

namespace Lisa.Bulder.WebApi
{
    [Route("[controller]")]
    public class SubscriptionsController : Controller
    {
        [HttpGet]
        public async Task<IActionResult> Get()
        {
            var subscriptions = await _db.FetchSubscriptions();
            return new ObjectResult(subscriptions);
        }

        [HttpGet("{id}", Name = "subscription")]
        public IActionResult Get(string id)
        {
            return new ObjectResult("");
        }

        [HttpPost("{channel}")]
        public async Task<IActionResult> Post([FromBody] SubscriptionEntity subscription, string channel)
        {
            subscription.PartitionKey = channel;

            if (!ModelState.IsValid)
            {
                return new BadRequestObjectResult(new { errorMessage = "Invalid json or url" });
            }

            string aFrom = "bulder@bulder.com";
            string aTo = subscription.EmailAddress;
            string aSubject = "Channel Subscription";
            string aBody = string.Format("You just subscribed to {0}", channel);

            //create the mail message
            MailMessage mail = new MailMessage();

            //set the addresses
            mail.From = new MailAddress(aFrom);
            mail.To.Add(aTo);

            //set the content
            mail.Subject = aSubject;
            mail.Body = aBody;

            //send the message
            SmtpClient smtp = new SmtpClient("localhost");
            smtp.UseDefaultCredentials = true;
            smtp.Send(mail);

            var createdSubscription = await _db.CreateSubscription(subscription);
            string location = Url.RouteUrl("subscription", new { id = createdSubscription.RowKey }, Request.Scheme);

            return new CreatedResult(location, createdSubscription);
        }
        private readonly Database _db = new Database();
    }
}