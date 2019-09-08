using MongoDB.Bson.Serialization;
using System;
using System.Collections.Generic;
using System.Text;

namespace NodaTime.MongoDBSerializers
{
    public static class NodaTimeSerializersRegister
    {
        public static void RegisterAll(IDateTimeZoneProvider provider)
        {
            ZonedDateTimeSerializer.RegisterSerializer(provider);

            LocalDateTimeSerializer.RegisterSerializer();
            LocalDateSerializer.RegisterSerializer();
            LocalTimeSerializer.RegisterSerializer();
           
            OffSetDatetimeSerializer.RegisterSerializer();
            OffSetTimeSerializer.RegisterSerializer();
            OffSetDateSerializer.RegisterSerializer();

            PeriodSerializer.RegisterSerializer();
            DurationSerializer.RegisterSerializer();
        }
    }
}
