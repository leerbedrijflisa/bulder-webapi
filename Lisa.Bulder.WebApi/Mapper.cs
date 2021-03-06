﻿using System;

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

        public static MessageEntity ToEntity(string channel, PostedMessage message)
        {
            return new MessageEntity
            {
                Text = message.Text,
                Author = message.Author,
                PartitionKey = channel,
                RowKey = String.Format("{0}{1}",
                    DateTime.MaxValue.Ticks - DateTime.Now.Ticks,
                    Guid.NewGuid())
            };
        }

        public static object ToChannel(object channelEntity)
        {
            var entity = (ChannelEntity) channelEntity;
            return new
            {
                Name = entity.Name,
                Id = entity.PartitionKey,
                Administrators = entity.Administrators,
                Authors = entity.Authors
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
    }
}