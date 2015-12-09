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

        //get a single subscription from a channel.
        [HttpGet("{channel}/{id}", Name = "subscription")]
        public async Task<IActionResult> Get(string channel, string id)
        {
            var subscription = await _db.FetchSubscription(channel, id);
            return new HttpOkObjectResult(subscription);
        }

        //get all subscriptions of a single channel
        [HttpGet("{channel}")]
        public async Task<IActionResult> Get(string channel)
        {
            var subscriptions = await _db.FetchSubscriptions(channel);
            return new ObjectResult(subscriptions);
        }

        //create a new subscription
        [HttpPost("{channel}")]
        public async Task<IActionResult> Post([FromBody] PostedSubscription subscription, string channel)
        {
            if (!ModelState.IsValid)
            {
                return new BadRequestObjectResult(new { errorMessage = "Invalid json or url" });
            }

            var subject = string.Format("You just subscribed to {0}", channel);
            var body = string.Format("You just subscribed to {0}", channel);

            var emailTask = _emailService.Send(subscription.EmailAddress, subject, body);
            var databaseTask = _db.CreateSubscription(channel, subscription);
            await Task.WhenAll(emailTask, databaseTask);

            dynamic createdSubscription = databaseTask.Result;
            string location = Url.RouteUrl("subscription", new { id = createdSubscription.EmailAddress }, Request.Scheme);

            return new CreatedResult(location, createdSubscription);
        }

        private readonly Database _db = new Database();
        private readonly IEmailService _emailService;
    }
}