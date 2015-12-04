using Microsoft.WindowsAzure.Storage;
using Microsoft.WindowsAzure.Storage.Table;

namespace Lisa.Bulder.WebApi
{
    public partial class Database
    {
        public Database()
        {
            var account = CloudStorageAccount.Parse("UseDevelopmentStorage=true");
            var client = account.CreateCloudTableClient();

            _messages = client.GetTableReference("messages");
            _channels = client.GetTableReference("channels");
            _subscriptions = client.GetTableReference("subscriptions");
            _users = client.GetTableReference("users");

            _channels.CreateIfNotExistsAsync();
            _messages.CreateIfNotExistsAsync();
            _users.CreateIfNotExistsAsync();
            _subscriptions.CreateIfNotExistsAsync();
        }
        
    }
}