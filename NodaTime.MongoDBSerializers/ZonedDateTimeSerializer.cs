using System;
using System.Collections.Generic;
using System.Text;
using MongoDB.Bson;
using MongoDB.Bson.IO;
using MongoDB.Bson.Serialization;
using NodaTime;
using NodaTime.Extensions;

namespace NodaTime.MongoDBSerializers
{
    public class ZonedDateTimeSerializer : IBsonSerializer<ZonedDateTime>
    {
        private IDateTimeZoneProvider _provider;

        public ZonedDateTimeSerializer(IDateTimeZoneProvider provider)
        {
            _provider = provider;
        }

        public Type ValueType => typeof(ZonedDateTime);

        public ZonedDateTime Deserialize(BsonDeserializationContext context, BsonDeserializationArgs args)
        {
            var bsonType = context.Reader.GetCurrentBsonType();

            if (bsonType != BsonType.Document)
            {
                throw new InvalidOperationException($"{bsonType} is not a document.");
            }

            context.Reader.ReadStartDocument();
            var timezoneId = context.Reader.ReadString("timezone");
            var time = context.Reader.ReadDateTime("datetime");
            var timezone = _provider.GetZoneOrNull(timezoneId);
            context.Reader.ReadEndDocument();

            if (timezone == null)
            {
                throw new Exception($"TimezoneID not found {timezoneId}");
            }

            return Instant.FromUnixTimeMilliseconds(time).InZone(timezone);
  
        }

        public void Serialize(BsonSerializationContext context, BsonSerializationArgs args, ZonedDateTime value)
        {
            context.Writer.WriteStartDocument();
            context.Writer.WriteString("timezone", value.Zone.Id);
            context.Writer.WriteDateTime("datetime", value.ToInstant().ToUnixTimeMilliseconds());
            context.Writer.WriteEndDocument();
        }

        
        public void Serialize(BsonSerializationContext context, BsonSerializationArgs args, object value)
        {
            if (value == null)
            {
                throw new ArgumentNullException(nameof(value));
            }

            if (!(value is ZonedDateTime))
            {
                throw new InvalidCastException(nameof(value));
            }

            Serialize(context, args, (ZonedDateTime)value);
            
        }
        object IBsonSerializer.Deserialize(BsonDeserializationContext context, BsonDeserializationArgs args)
        {
            return Deserialize(context, args);
        }

        public static ZonedDateTimeSerializer RegisterSerializer(IDateTimeZoneProvider provider)
        {
            ZonedDateTimeSerializer serializer = new ZonedDateTimeSerializer(provider);
            BsonSerializer.RegisterSerializer(typeof(ZonedDateTime), serializer);
            return serializer;
        }
    }
}
