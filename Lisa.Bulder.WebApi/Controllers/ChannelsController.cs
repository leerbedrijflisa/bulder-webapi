using Microsoft.AspNet.Mvc;
using System.Threading.Tasks;

namespace Lisa.Bulder.WebApi
{
    [Route("[controller]")]
    public class ChannelsController : Controller
    {
        //Get all channels
        [HttpGet]
        public async Task<IActionResult> Get()
        {
            var channels = await _db.FetchChannels();
            return new HttpOkObjectResult(channels);
        }
        
        //TODO: GET channels/{channel} laad alle berichten uit. Dit moet alleen de berichten van het specifieke kanaal uitladen.
        [HttpGet("{id}", Name = "channel")]
        public async Task<IActionResult> Get(string id)
        {
            var channels = await _db.FetchChannels();
            return new HttpOkObjectResult(channels);
        }

        //Create a channel
        [HttpPost]
        public async Task<IActionResult> Post([FromBody] PostedChannel channel)
        {
            if (!ModelState.IsValid)
            {
                return new BadRequestObjectResult(new { errorMessage = "Invalid json or url" });
            }

            dynamic createdChannel = await _db.CreateChannel(channel);
            string location = Url.RouteUrl("channel", new { id = createdChannel.Id }, Request.Scheme);

            return new CreatedResult(location, createdChannel);
        }

        private readonly Database _db = new Database();
    }
}