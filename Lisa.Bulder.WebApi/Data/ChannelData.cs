using Microsoft.WindowsAzure.Storage.Table;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Lisa.Bulder.WebApi
{
    partial class Database
    {
        public async Task<object> FetchChannel(string channel, string id)
        {
            var operation = TableOperation.Retrieve<MessageEntity>(channel, id);
            var result = await _messages.ExecuteAsync(operation);
            return Mapper.ToMessage(result.Result);
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
