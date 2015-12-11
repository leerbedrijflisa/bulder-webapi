using Microsoft.WindowsAzure.Storage.Table;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Lisa.Bulder.WebApi
{
    partial class Database
    {
        public async Task<object> FetchChannel(string channel, string id)
        {
            //var operation = TableOperation.Retrieve<ChannelEntity>(id, null);
            //var result = await _channels.ExecuteAsync(operation);
            //return Mapper.ToChannel(result.Result);

            var operation2 = TableOperation.Retrieve<MessageEntity>(channel, id);
            var result2 = await _messages.ExecuteAsync(operation2);
            return Mapper.ToMessage(result2.Result);

            //var query = new TableQuery<ChannelEntity>().Where(TableQuery.GenerateFilterCondition("PartitionKey", QueryComparisons.Equal, channel.ToLower()));
            //var segment = await _channels.ExecuteQuerySegmentedAsync(query, null);
            //return Mapper.ToChannel(segment.Results);
        }

        public async Task<IEnumerable<object>> FetchChannels()
        {
            var query = new TableQuery<ChannelEntity>();
            var segment = await _channels.ExecuteQuerySegmentedAsync(query, null);
            return Mapper.ToChannels(segment);
        }

        public async Task<object> CreateChannel(PostedChannel channel)
        {
            var entity = Mapper.ToEntity(channel);
            var operation = TableOperation.Insert(entity);
            var result = await _channels.ExecuteAsync(operation);
            return Mapper.ToChannel((ChannelEntity) result.Result);
        }

        private CloudTable _channels;
    }
}
