using Microsoft.AspNet.Mvc;
using System.Threading.Tasks;

namespace Lisa.Bulder.WebApi
{
    [Route("[controller]")]
    public class MessagesController : Controller
    {
        //Get a single message from a channel
        [HttpGet("{channel}/{id}", Name = "message")]
        public async Task<IActionResult> Get(string channel, string id)
        {
            var message = await _db.FetchMessage(channel, id);
            return new HttpOkObjectResult(message);
        }

        //Get all messages from a channel
        [HttpGet("{channel}")]
        public async Task<IActionResult> Get(string channel)
        {
            var messages = await _db.FetchMessages(channel);
            return new HttpOkObjectResult(messages);
        }

        //Create message
        [HttpPost("{channel}")]
        public async Task<IActionResult> Post([FromBody] PostedMessage message, string channel)
        {

            if (!ModelState.IsValid)
            {
                return new BadRequestObjectResult(new { errorMessage = "Invalid json or url" });
            }

            dynamic createdMessage = await _db.CreateMessage(channel, message);
            string location = Url.RouteUrl("message", new { id = createdMessage.Id }, Request.Scheme);

            return new CreatedResult(location, createdMessage);
        }

        private readonly Database _db = new Database();
    }
}
