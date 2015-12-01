using System;

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
    }
}