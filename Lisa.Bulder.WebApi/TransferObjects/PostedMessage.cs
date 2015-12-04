namespace Lisa.Bulder.WebApi
{
    // This is the message that clients send to the Web API.
    public class PostedMessage
    {
        public string Text { get; set; }
        public string Author { get; set; }
    }
}