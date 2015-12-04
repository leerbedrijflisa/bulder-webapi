using System.ComponentModel.DataAnnotations;

namespace Lisa.Bulder.WebApi
{
    public class PostedChannel
    {
        [Required]
        public string Administrators { get; set; }
        [Required]
        public string Authors { get; set; }
        [Required]
        public string Name { get; set; }
    }
}