using Microsoft.AspNet.Mvc;
using SendGrid;
using System.Net;
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

            ////Locally vis smtp4dev
            //string aFrom = "bulder@bulder.com";
            string aTo = subscription.EmailAddress;
            //string aSubject = "Channel Subscription";
            //string aBody = string.Format("You just subscribed to {0}", channel);

            ////create the mail message
            //MailMessage mail = new MailMessage();

            ////set the addresses
            //mail.From = new MailAddress(aFrom);
            //mail.To.Add(aTo);

            ////set the content
            //mail.Subject = aSubject;
            //mail.Body = aBody;

            ////send the message
            //SmtpClient smtp = new SmtpClient("localhost");
            //smtp.UseDefaultCredentials = true;
            //smtp.Send(mail);

            //Live via SendGrid
            SendGridMessage myMessage = new SendGridMessage();
            myMessage.AddTo(aTo);
            myMessage.From = new MailAddress("noreply@bulder.com", "Mike Faro");
            myMessage.Subject = "Channel Subscription";
            myMessage.Text = string.Format("You just subscribed to {0}", channel);

            // Create credentials, specifying your user name and password.
            var credentials = new NetworkCredential("azure_0d222e73bb333249b0e60eb91c9ace1f@azure.com", "qDoHuykdvN5&D@uamz^zyMlaAYq2&6BA9s$EJjhx3jutJ9r3W");

            // Create an Web transport for sending email, using credentials...
            var transportWeb = new Web(credentials);

            // ...OR create a Web transport, using API Key (preferred)
            //var transportWeb = new Web("");

            // Send the email.
            await transportWeb.DeliverAsync(myMessage);

            var createdSubscription = await _db.CreateSubscription(subscription);
            string location = Url.RouteUrl("subscription", new { id = createdSubscription.RowKey }, Request.Scheme);

            return new CreatedResult(location, createdSubscription);
        }
        private readonly Database _db = new Database();
    }
}