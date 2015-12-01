using Microsoft.WindowsAzure.Storage.Table;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Lisa.Bulder.WebApi
{
    partial class Database
    {
        public async Task<IEnumerable<ChannelEntity>> FetchChannels()
        {
            var query = new TableQuery<ChannelEntity>();
            var segment = await _channels.ExecuteQuerySegmentedAsync(query, null);
            return segment;
        }

        public async Task<ChannelEntity> CreateChannel(ChannelEntity channel)
        {
            channel.Name = channel.PartitionKey;
            channel.PartitionKey = channel.Name.ToLower();
            channel.RowKey = string.Empty;
            var operation = TableOperation.Insert(channel);
            var result = await _channels.ExecuteAsync(operation);
            return (ChannelEntity) result.Result;
        }
    }
}
