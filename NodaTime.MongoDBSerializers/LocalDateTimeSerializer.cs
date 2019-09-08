using System;
using System.Collections.Generic;
using System.Text;
using MongoDB.Bson.Serialization;
using NodaTime;

namespace NodaTime.MongoDBSerializers
{
    public class LocalDateTimeSerializer : IBsonSerializer<LocalDateTime>
    {
        public Type ValueType => typeof(LocalDateTime);

        public LocalDateTime Deserialize(BsonDeserializationContext context, BsonDeserializationArgs args)
        {
            var value = context.Reader.ReadDateTime();
            return Instant.FromUnixTimeMilliseconds(value).InUtc().LocalDateTime;
        }

        public void Serialize(BsonSerializationContext context, BsonSerializationArgs args, LocalDateTime value)
        {
            context.Writer.WriteDateTime(value.InUtc().ToInstant().ToUnixTimeMilliseconds());
        }

        public void Serialize(BsonSerializationContext context, BsonSerializationArgs args, object value)
        {
            if (value == null)
            {
                throw new ArgumentNullException(nameof(value));
            }

            if (!(value is LocalDateTime))
            {
                throw new InvalidCastException(nameof(value));
            }

            Serialize(context, args, (LocalDateTime)value);
        }

        object IBsonSerializer.Deserialize(BsonDeserializationContext context, BsonDeserializationArgs args)
        {
            return Deserialize(context, args);
        }

        public static LocalDateTimeSerializer RegisterSerializer()
        {
            LocalDateTimeSerializer serializer = new LocalDateTimeSerializer();
            BsonSerializer.RegisterSerializer(typeof(LocalDateTime), serializer);
            return serializer;
        }
    }
}
