using Microsoft.WindowsAzure.Storage;
using Microsoft.WindowsAzure.Storage.Table;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Lisa.Bulder.WebApi
{
    partial class Database
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

        //Messages
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
            return segment;
        }

        public async Task<object> CreateMessage(string channel, PostedMessage message)
        {
            var entity = Mapper.ToEntity(channel, message);
            var operation = TableOperation.Insert(entity);
            var result = await _messages.ExecuteAsync(operation);
            return Mapper.ToMessage((MessageEntity) result.Result);
        }

        //Subscriptions
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

        //Users
        public async Task<IEnumerable<UserEntity>> FetchUsers()
        {
            var query = new TableQuery<UserEntity>();
            var segment = await _users.ExecuteQuerySegmentedAsync(query, null);
            return segment;
        }
        
        public async Task<UserEntity> CreateUser(UserEntity user)
        {
            user.PartitionKey = user.PartitionKey;
            user.RowKey = Guid.NewGuid().ToString();
            var operation = TableOperation.Insert(user);
            var result = await _users.ExecuteAsync(operation);
            return (UserEntity) result.Result;
        }

        private CloudTable _messages;
        private CloudTable _channels;
        private CloudTable _subscriptions;
        private CloudTable _users;
    }
}