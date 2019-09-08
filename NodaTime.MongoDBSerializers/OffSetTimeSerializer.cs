using MongoDB.Bson;
using MongoDB.Bson.IO;
using MongoDB.Bson.Serialization;
using NodaTime;
using System;
using System.Collections.Generic;
using System.Text;

namespace NodaTime.MongoDBSerializers
{
    public class OffSetTimeSerializer : IBsonSerializer<OffsetTime>
    {
        public Type ValueType => typeof(OffsetTime);

        public OffsetTime Deserialize(BsonDeserializationContext context, BsonDeserializationArgs args)
        {
            var bsonType = context.Reader.GetCurrentBsonType();

            if (bsonType != BsonType.Document)
            {
                throw new InvalidOperationException($"{bsonType} is not a document.");
            }
            context.Reader.ReadStartDocument();
            var dtValue = context.Reader.ReadDateTime("time");
            var offSetNanoSeconds = context.Reader.ReadInt64("offset");
            context.Reader.ReadEndDocument();

            return Instant.FromUnixTimeMilliseconds(dtValue).WithOffset(Offset.FromNanoseconds(offSetNanoSeconds)).ToOffsetTime();
        }

        public void Serialize(BsonSerializationContext context, BsonSerializationArgs args, OffsetTime value)
        {
            context.Writer.WriteStartDocument();
            context.Writer.WriteDateTime("time", value.On(new LocalDate(1, 1, 1)).ToInstant().ToUnixTimeMilliseconds());
            context.Writer.WriteInt64("offset", value.Offset.Nanoseconds);
            context.Writer.WriteEndDocument();
        }

        public void Serialize(BsonSerializationContext context, BsonSerializationArgs args, object value)
        {
            if (value == null)
            {
                throw new ArgumentNullException(nameof(value));
            }

            if (!(value is OffsetTime))
            {
                throw new InvalidCastException(nameof(value));
            }

            Serialize(context, args, (OffsetTime)value);
        }

        object IBsonSerializer.Deserialize(BsonDeserializationContext context, BsonDeserializationArgs args)
        {
            return Deserialize(context, args);
        }

        public static OffSetTimeSerializer RegisterSerializer()
        {
            OffSetTimeSerializer serializer = new OffSetTimeSerializer();
            BsonSerializer.RegisterSerializer(typeof(OffsetTime), serializer);
            return serializer;
        }
    }
   
}
