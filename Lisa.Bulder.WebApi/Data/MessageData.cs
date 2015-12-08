using Microsoft.WindowsAzure.Storage.Table;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Lisa.Bulder.WebApi
{
    partial class Database
    {
        public async Task<object> FetchMessage(string channel, string id)
        {
            var operation = TableOperation.Retrieve<MessageEntity>(channel, id);
            var result = await _messages.ExecuteAsync(operation);
            return Mapper.ToMessage(result.Result);
        }

        public async Task<IEnumerable<object>> FetchMessages(string channel)
        {
            var query = new TableQuery<MessageEntity>().Where(TableQuery.GenerateFilterCondition("PartitionKey", QueryComparisons.Equal, channel));
            var segment = await _messages.ExecuteQuerySegmentedAsync(query, null);
            return Mapper.ToMessages(segment);
        }

        public async Task<object> CreateMessage(string channel, PostedMessage message)
        {
            var entity = Mapper.ToEntity(channel, message);
            var operation = TableOperation.Insert(entity);
            var result = await _messages.ExecuteAsync(operation);
            return Mapper.ToMessage((MessageEntity) result.Result);
        }

        private CloudTable _messages;
    }
}
