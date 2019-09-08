using System;
using System.Collections.Generic;
using System.Text;
using MongoDB.Bson.Serialization;
using NodaTime;

namespace NodaTime.MongoDBSerializers
{
    public class LocalTimeSerializer : IBsonSerializer<LocalTime>
    {
        public Type ValueType => typeof(LocalTime);

        public LocalTime Deserialize(BsonDeserializationContext context, BsonDeserializationArgs args)
        {
            var value = context.Reader.ReadDateTime();
            return Instant.FromUnixTimeMilliseconds(value).InUtc().TimeOfDay;
        }

        public void Serialize(BsonSerializationContext context, BsonSerializationArgs args, LocalTime value)
        {
            context.Writer.WriteDateTime(value.On(new LocalDate(1,1,1)).InUtc().ToInstant().ToUnixTimeMilliseconds());
        }

        public void Serialize(BsonSerializationContext context, BsonSerializationArgs args, object value)
        {
            if (value == null)
            {
                throw new ArgumentNullException(nameof(value));
            }

            if (!(value is LocalTime))
            {
                throw new InvalidCastException(nameof(value));
            }

            Serialize(context, args, (LocalTime)value);
        }

        object IBsonSerializer.Deserialize(BsonDeserializationContext context, BsonDeserializationArgs args)
        {
            return Deserialize(context, args);
        }

        public static LocalTimeSerializer RegisterSerializer()
        {
            LocalTimeSerializer serializer = new LocalTimeSerializer();
            BsonSerializer.RegisterSerializer(typeof(LocalTime), serializer);
            return serializer;
        }
    }
}
