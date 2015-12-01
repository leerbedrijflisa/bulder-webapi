using Microsoft.AspNet.Mvc;
using System.Threading.Tasks;

namespace Lisa.Bulder.WebApi
{
    [Route("[controller]")]
    public class SubscriptionsController : Controller
    {
        public SubscriptionsController(IEmailService emailService, Database database)
        {
            _emailService = emailService;
            _db = database;
        }

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

            var subject = "Channel Subscription";
            var body = string.Format("You just subscribed to {0}", channel);

            var emailTask = _emailService.Send(subscription.EmailAddress, subject, body);
            var databaseTask = _db.CreateSubscription(subscription);
            Task.WaitAll(emailTask, databaseTask);
            var createdSubscription = databaseTask.Result;

            string location = Url.RouteUrl("subscription", new { id = createdSubscription.RowKey }, Request.Scheme);

            return new CreatedResult(location, createdSubscription);
        }
        private readonly Database _db = new Database();
        private readonly IEmailService _emailService;
    }
}