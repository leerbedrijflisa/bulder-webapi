using System.ComponentModel.DataAnnotations;

namespace Lisa.Bulder.WebApi
{
    // This is the message that clients send to the Web API.
    public class PostedMessage
    {
        [Required]
        public string Text { get; set; }
        [Required]
        public string Author { get; set; }
    }
}