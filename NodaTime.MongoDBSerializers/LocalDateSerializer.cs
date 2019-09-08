using System;
using System.Collections.Generic;
using System.Text;
using MongoDB.Bson.Serialization;
using NodaTime;

namespace NodaTime.MongoDBSerializers
{
    public class LocalDateSerializer : IBsonSerializer<LocalDate>
    {
        public Type ValueType => typeof(LocalDate);

        public LocalDate Deserialize(BsonDeserializationContext context, BsonDeserializationArgs args)
        {
            var value = context.Reader.ReadDateTime();
            return Instant.FromUnixTimeMilliseconds(value).InUtc().Date;
        }

        public void Serialize(BsonSerializationContext context, BsonSerializationArgs args, LocalDate value)
        {
            context.Writer.WriteDateTime(value.At(LocalTime.MinValue).InUtc().ToInstant().ToUnixTimeMilliseconds());
        }

        public void Serialize(BsonSerializationContext context, BsonSerializationArgs args, object value)
        {
            if (value == null || !(value is LocalDate))
            {
                throw new ArgumentNullException(nameof(value));
            }

            if (!(value is LocalDate))
            {
                throw new InvalidCastException(nameof(value));
            }

            Serialize(context, args, (LocalDate)value);
        }

        object IBsonSerializer.Deserialize(BsonDeserializationContext context, BsonDeserializationArgs args)
        {
            return Deserialize(context, args);
        }

        public static LocalDateSerializer RegisterSerializer()
        {
            LocalDateSerializer serializer = new LocalDateSerializer();
            BsonSerializer.RegisterSerializer(typeof(LocalDate), serializer);
            return serializer;
        }
    }
}
