using Microsoft.WindowsAzure.Storage.Table;
using System;
using System.Collections.Generic;

namespace Lisa.Bulder.WebApi
{
    public static class Mapper
    {
        public static object ToMessage(object messageEntity)
        {
            var entity = (MessageEntity) messageEntity;
            return new
            {
                Id = entity.RowKey,
                Text = entity.Text,
                Author = entity.Author,
                Posted = entity.Timestamp
            };
        }

        public static List<object> ToMessages(TableQuerySegment<MessageEntity> messageEntity)
        {
            List<object> messageEntities = new List<object>();
            foreach (var message in messageEntity)
            {
                messageEntities.Add(ToMessage(message));
            }
            return messageEntities;
        }

        public static object ToChannel(object channelEntity)
        {
            var entity = (ChannelEntity) channelEntity;
            return new
            {
                Name = entity.Name,
                Id = entity.PartitionKey.ToLower(),
                Administrators = entity.Administrators,
                Authors = entity.Authors
            };
        }

        public static List<object> ToChannels(TableQuerySegment<ChannelEntity> channelEntity)
        {
            List<object> channelEntities = new List<object>();
            foreach (var channel in channelEntity)
            {
                channelEntities.Add(ToChannel(channel));
            }
            return channelEntities;
        }

        public static object ToSubscription(object subscriptionEntity)
        {
            var entity = (SubscriptionEntity) subscriptionEntity;
            return new
            {
                EmailAddress = entity.EmailAddress
            };
        }

        public static List<object> ToSubscriptions(TableQuerySegment<SubscriptionEntity> subscriptionEntity)
        {
            List<object> subscriptionEntities = new List<object>();
            foreach (var subscription in subscriptionEntity)
            {
                subscriptionEntities.Add(ToSubscription(subscription));
            }
            return subscriptionEntities;
        }

        public static MessageEntity ToEntity(string channel, PostedMessage message)
        {
            return new MessageEntity
            {
                Text = message.Text,
                Author = message.Author,
                PartitionKey = channel.ToLower(),
                RowKey = String.Format("{0}{1}",
                    DateTime.MaxValue.Ticks - DateTime.Now.Ticks,
                    Guid.NewGuid())
            };
        }

        public static ChannelEntity ToEntity(PostedChannel channel)
        {
            return new ChannelEntity
            {
                Name = channel.Name,
                PartitionKey = channel.Name.ToLower(),
                RowKey = string.Empty,
                Administrators = channel.Administrators,
                Authors = channel.Authors
            };
        }

        public static SubscriptionEntity ToEntity(string channel, PostedSubscription subscription)
        {
            return new SubscriptionEntity
            {
                EmailAddress = subscription.EmailAddress,
                PartitionKey = channel.ToLower(),
                RowKey = Guid.NewGuid().ToString()
                
            };
        }
    }
}