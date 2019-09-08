using System;
using System.Collections.Generic;
using System.Text;
using MongoDB.Bson;
using MongoDB.Bson.IO;
using MongoDB.Bson.Serialization;
using NodaTime;

namespace NodaTime.MongoDBSerializers
{
    public class PeriodSerializer : IBsonSerializer<Period>
    {
        public Type ValueType => typeof(Period);

        public Period Deserialize(BsonDeserializationContext context, BsonDeserializationArgs args)
        {
            var bsonType = context.Reader.GetCurrentBsonType();

            if (bsonType != BsonType.Document)
            {
                throw new InvalidOperationException($"{bsonType} is not a document.");
            }

            context.Reader.ReadStartDocument();
            
            var years = context.Reader.ReadInt32("years");
            var months = context.Reader.ReadInt32("months");
            var weeks = context.Reader.ReadInt32("weeks");
            var days = context.Reader.ReadInt32("days");
            var hours = context.Reader.ReadInt64("hours");
            var minutes = context.Reader.ReadInt64("minutes");
            var seconds = context.Reader.ReadInt64("seconds");
            var milliseconds = context.Reader.ReadInt64("milliseconds");
            var ticks = context.Reader.ReadInt64("ticks");
            var nanoseconds = context.Reader.ReadInt64("nanoseconds");
            context.Reader.ReadEndDocument();

            return new PeriodBuilder()
            {
                Years = years,
                Months = months,
                Weeks = weeks,
                Days = days,
                Hours = hours,
                Minutes = minutes,
                Seconds = seconds,
                Milliseconds = milliseconds,
                Ticks = ticks,
                Nanoseconds = nanoseconds
            }.Build();

        }

        public void Serialize(BsonSerializationContext context, BsonSerializationArgs args, Period value)
        {
            context.Writer.WriteStartDocument();
            context.Writer.WriteInt32("years", value.Years);
            context.Writer.WriteInt32("months", value.Months);
            context.Writer.WriteInt32("weeks", value.Weeks);
            context.Writer.WriteInt32("days", value.Days);
            context.Writer.WriteInt64("hours", value.Hours);
            context.Writer.WriteInt64("minutes", value.Minutes);
            context.Writer.WriteInt64("seconds", value.Seconds);
            context.Writer.WriteInt64("milliseconds", value.Milliseconds);
            context.Writer.WriteInt64("ticks", value.Ticks);
            context.Writer.WriteInt64("nanoseconds", value.Nanoseconds);
            context.Writer.WriteEndDocument();
        }

        public void Serialize(BsonSerializationContext context, BsonSerializationArgs args, object value)
        {
            if (value == null)
            {
                throw new ArgumentNullException(nameof(value));
            }

            if (!(value is Period))
            {
                throw new InvalidCastException(nameof(value));
            }

            Serialize(context, args, (Period)value);
        }

        object IBsonSerializer.Deserialize(BsonDeserializationContext context, BsonDeserializationArgs args)
        {
            return Deserialize(context, args);
        }

        public static PeriodSerializer RegisterSerializer()
        {
            PeriodSerializer serializer = new PeriodSerializer();
            BsonSerializer.RegisterSerializer(typeof(Period), serializer);
            return serializer;
        }
    }
}
