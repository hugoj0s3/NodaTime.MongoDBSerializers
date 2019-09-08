using MongoDB.Bson;
using MongoDB.Bson.IO;
using MongoDB.Bson.Serialization;
using NodaTime;
using System;
using System.Collections.Generic;
using System.Text;

namespace NodaTime.MongoDBSerializers
{
    public class OffSetDateSerializer : IBsonSerializer<OffsetDate>
    {
        public Type ValueType => typeof(OffsetDate);

        public OffsetDate Deserialize(BsonDeserializationContext context, BsonDeserializationArgs args)
        {
            var bsonType = context.Reader.GetCurrentBsonType();

            if (bsonType != BsonType.Document)
            {
                throw new InvalidOperationException($"{bsonType} is not a document.");
            }

            context.Reader.ReadStartDocument();
            var dtValue = context.Reader.ReadDateTime("date");
            var offSetNanoSeconds = context.Reader.ReadInt64("offset");
            context.Reader.ReadEndDocument();

            return Instant.FromUnixTimeMilliseconds(dtValue).WithOffset(Offset.FromNanoseconds(offSetNanoSeconds)).ToOffsetDate();
        }

        public void Serialize(BsonSerializationContext context, BsonSerializationArgs args, OffsetDate value)
        {
            context.Writer.WriteStartDocument();
            context.Writer.WriteDateTime("date", value.At(LocalTime.MinValue).ToInstant().ToUnixTimeMilliseconds());
            context.Writer.WriteInt64("offset", value.Offset.Nanoseconds);
            context.Writer.WriteEndDocument();
        }

        public void Serialize(BsonSerializationContext context, BsonSerializationArgs args, object value)
        {
            if (value == null)
            {
                throw new ArgumentNullException(nameof(value));
            }

            if (!(value is OffsetDate))
            {
                throw new InvalidCastException(nameof(value));
            }

            Serialize(context, args, (OffsetTime) value);
        }

        object IBsonSerializer.Deserialize(BsonDeserializationContext context, BsonDeserializationArgs args)
        {
            return Deserialize(context, args);
        }

        public static OffSetDateSerializer RegisterSerializer()
        {
            OffSetDateSerializer serializer = new OffSetDateSerializer();
            BsonSerializer.RegisterSerializer(typeof(OffsetDate), serializer);
            return serializer;
        }

    }
}
