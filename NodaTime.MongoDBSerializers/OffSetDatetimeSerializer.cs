using System;
using System.Collections.Generic;
using System.Text;
using MongoDB.Bson;
using MongoDB.Bson.IO;
using MongoDB.Bson.Serialization;
using NodaTime;

namespace NodaTime.MongoDBSerializers
{
    public class OffSetDatetimeSerializer : IBsonSerializer<OffsetDateTime>
    {
        public Type ValueType => typeof(OffsetDateTime);

        public OffsetDateTime Deserialize(BsonDeserializationContext context, BsonDeserializationArgs args)
        {
            var bsonType = context.Reader.GetCurrentBsonType();

            if (bsonType != BsonType.Document)
            {
                throw new InvalidOperationException($"{bsonType} is not a document.");
            }

            context.Reader.ReadStartDocument();
            var dtValue = context.Reader.ReadDateTime("datetime");
            var offSetNanoSeconds = context.Reader.ReadInt64("offset");
            context.Reader.ReadEndDocument();

            return Instant.FromUnixTimeMilliseconds(dtValue).WithOffset(Offset.FromNanoseconds(offSetNanoSeconds));
        }

        public void Serialize(BsonSerializationContext context, BsonSerializationArgs args, OffsetDateTime value)
        {
            context.Writer.WriteStartDocument();
            context.Writer.WriteDateTime("datetime", value.ToInstant().ToUnixTimeMilliseconds());
            context.Writer.WriteInt64("offset",value.Offset.Nanoseconds);
            context.Writer.WriteEndDocument();
        }

        public void Serialize(BsonSerializationContext context, BsonSerializationArgs args, object value)
        {
            if (value == null)
            {
                throw new ArgumentNullException(nameof(value));
            }

            if (!(value is OffsetDateTime))
            {
                throw new InvalidCastException(nameof(value));
            }

            Serialize(context, args, (OffsetDateTime)value);
        }

        object IBsonSerializer.Deserialize(BsonDeserializationContext context, BsonDeserializationArgs args)
        {
            return Deserialize(context, args);
        }

        public static OffSetDatetimeSerializer RegisterSerializer()
        {
            OffSetDatetimeSerializer serializer = new OffSetDatetimeSerializer();
            BsonSerializer.RegisterSerializer(typeof(OffsetDateTime), serializer);
            return serializer;
        }
    }
}
