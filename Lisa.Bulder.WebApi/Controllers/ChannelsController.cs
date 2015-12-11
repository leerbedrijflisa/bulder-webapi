using Microsoft.AspNet.Mvc;
using System.Threading.Tasks;

namespace Lisa.Bulder.WebApi
{
    [Route("[controller]")]
    public class ChannelsController : Controller
    {
        //TODO: GET channels/{channel} laad alle berichten uit. Dit moet alleen de berichten van het specifieke kanaal uitladen.

        // get info and messages from the channel
        [HttpGet("{channel}", Name = "channel")]
        public async Task<IActionResult> Get(string channel)
        {
            var messages = await _db.FetchMessages(channel);
            return new HttpOkObjectResult(messages);
        }

        // get all channels
        [HttpGet]
        public async Task<IActionResult> Get()
        {
            var channels = await _db.FetchChannels();
            return new HttpOkObjectResult(channels);
        }
        
        // create a channel
        [HttpPost]
        public async Task<IActionResult> Post([FromBody] PostedChannel channel)
        {
            if (!ModelState.IsValid)
            {
                return new BadRequestObjectResult(new { errorMessage = "Invalid json or url" });
            }

            dynamic createdChannel = await _db.CreateChannel(channel);
            string location = Url.RouteUrl("channel", new { channel = createdChannel.Id }, Request.Scheme);

            return new CreatedResult(location, createdChannel);
        }

        private readonly Database _db = new Database();
    }
}