using System.ComponentModel.DataAnnotations;

namespace Lisa.Bulder.WebApi
{
    public class PostedSubscription
    {
        [Required]
        public string EmailAddress { get; set; }
    }
}