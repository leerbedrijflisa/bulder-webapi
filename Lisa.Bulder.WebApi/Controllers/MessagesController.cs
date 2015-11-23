using Microsoft.AspNet.Mvc;
using System.Threading.Tasks;

namespace Lisa.Bulder.WebApi
{
    [Route("[controller]")]
    public class MessagesController : Controller
    {
        //Get all messages
        [HttpGet]
        public async Task<IActionResult> Get()
        {
            var messages = await _db.FetchMessages();
            return new ObjectResult(messages);
        }

        [HttpGet("{id}", Name = "message")]
        public IActionResult Get(string id)
        {
            return new ObjectResult("");
        }

        //Create message
        [HttpPost("{channel}")]
        public async Task<IActionResult> Post([FromBody] MessageEntity message, string channel)
        {
            message.PartitionKey = channel;

            if (!ModelState.IsValid)
            {
                return new BadRequestObjectResult(new { errorMessage = "Invalid json or url" });
            }

            var createdMessage = await _db.CreateMessage(message);
            string location = Url.RouteUrl("message", new { id = createdMessage.RowKey }, Request.Scheme);

            return new CreatedResult(location, createdMessage);
        }

        private readonly Database _db = new Database();
    }
}
