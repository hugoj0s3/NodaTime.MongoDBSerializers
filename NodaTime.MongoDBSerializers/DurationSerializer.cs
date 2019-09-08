using System;
using System.Collections.Generic;
using System.Text;
using MongoDB.Bson.Serialization;
using NodaTime;

namespace NodaTime.MongoDBSerializers
{
    public class DurationSerializer : IBsonSerializer<Duration>
    {
        public Type ValueType => typeof(Duration);

        public Duration Deserialize(BsonDeserializationContext context, BsonDeserializationArgs args)
        {
            var duration = context.Reader.ReadDouble();
            return Duration.FromNanoseconds(duration);
        }

        public void Serialize(BsonSerializationContext context, BsonSerializationArgs args, Duration value)
        {
            context.Writer.WriteDouble(value.TotalNanoseconds);
        }

        public void Serialize(BsonSerializationContext context, BsonSerializationArgs args, object value)
        {
            if (value == null)
            {
                throw new ArgumentNullException(nameof(value));
            }

            if (!(value is Duration))
            {
                throw new InvalidCastException(nameof(value));
            }

            Serialize(context, args, (Duration)value);
        }

        object IBsonSerializer.Deserialize(BsonDeserializationContext context, BsonDeserializationArgs args)
        {
            return Deserialize(context, args);
        }

        public static DurationSerializer RegisterSerializer()
        {
            DurationSerializer serializer = new DurationSerializer();
            BsonSerializer.RegisterSerializer(typeof(Duration), serializer);
            return serializer;
        }
    }
}
