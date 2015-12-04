using Microsoft.WindowsAzure.Storage.Table;
using System.ComponentModel.DataAnnotations;

namespace Lisa.Bulder.WebApi
{
    public class ChannelEntity : TableEntity
    {
        [Required]
        public string Administrators { get; set; }
        [Required]
        public string Authors { get; set; }
        [Required]
        public string Name { get; set; }
    }
}