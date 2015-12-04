using Microsoft.WindowsAzure.Storage.Table;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Lisa.Bulder.WebApi
{
    partial class Database
    {
        public async Task<IEnumerable<SubscriptionEntity>> FetchSubscriptions()
        {
            var query = new TableQuery<SubscriptionEntity>();
            var segment = await _subscriptions.ExecuteQuerySegmentedAsync(query, null);
            return segment;
        }

        public async Task<SubscriptionEntity> CreateSubscription(SubscriptionEntity subscription)
        {
            subscription.PartitionKey = subscription.PartitionKey;
            subscription.RowKey = Guid.NewGuid().ToString();
            var operation = TableOperation.Insert(subscription);
            var result = await _subscriptions.ExecuteAsync(operation);
            return (SubscriptionEntity) result.Result;
        }

        private CloudTable _subscriptions;
    }
}
