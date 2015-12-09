using Microsoft.WindowsAzure.Storage.Table;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Lisa.Bulder.WebApi
{
    partial class Database
    {
        public async Task<object> FetchSubscription(string channel, string id)
        {
            var operation = TableOperation.Retrieve<SubscriptionEntity>(channel, id);
            var result = await _subscriptions.ExecuteAsync(operation);
            return Mapper.ToSubscription(result.Result);
        }

        public async Task<IEnumerable<object>> FetchSubscriptions(string channel)
        {
            var query = new TableQuery<SubscriptionEntity>().Where(TableQuery.GenerateFilterCondition("PartitionKey", QueryComparisons.Equal, channel));
            var segment = await _subscriptions.ExecuteQuerySegmentedAsync(query, null);
            return Mapper.ToSubscriptions(segment);
        }

        public async Task<object> CreateSubscription(string channel, PostedSubscription subscription)
        {
            var entity = Mapper.ToEntity(channel, subscription);
            var operation = TableOperation.Insert(entity);
            var result = await _subscriptions.ExecuteAsync(operation);
            return Mapper.ToSubscription((SubscriptionEntity) result.Result);
        }

        private CloudTable _subscriptions;
    }
}
