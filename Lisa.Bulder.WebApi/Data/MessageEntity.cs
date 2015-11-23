using Microsoft.WindowsAzure.Storage.Table;
using System.ComponentModel.DataAnnotations;

namespace Lisa.Bulder.WebApi
{
    public class MessageEntity : TableEntity
    {
        [Required]
        public string Text { get; set; }
        [Required]
        public string Author { get; set; }
    }
}