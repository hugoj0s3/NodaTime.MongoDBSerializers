using MongoDB.Driver;
using System;
using MongoDB.Bson;
using MongoDB.Bson.Serialization;
using Shouldly;
using Xunit;

namespace NodaTime.MongoDBSerializers.Tests
{
    public class Tests
    {
        private static IDateTimeZoneProvider provider = DateTimeZoneProviders.Tzdb;

        [Fact]
        public void Test_DataSerializerAndDeserializeReturnsExpcetedResult()
        {
            //Arrange
            NodaTimeSerializersRegister.RegisterAll(provider);
            var data = new Data();
            var id = data.Id;

            // Action
            var dataRetrieved = BsonSerializer.Deserialize<Data>(data.ToBson());

            //Assert
            dataRetrieved.Created.ShouldBe(data.Created);
            dataRetrieved.CreatedDate.ShouldBe(data.CreatedDate);
            dataRetrieved.CreatedDateTime.ShouldBe(data.CreatedDateTime);
            dataRetrieved.CreatedTime.ShouldBe(data.CreatedTime);
            dataRetrieved.CreatedOffsetDateTime.ShouldBe(data.CreatedOffsetDateTime);
            dataRetrieved.CreatedOffsetTime.ShouldBe(data.CreatedOffsetTime);
            dataRetrieved.Period.ShouldBe(data.Period);
            dataRetrieved.Duration.ShouldBe(data.Duration);
       
        }

        private class Data
        {
            public Data()
            {
                Id = Guid.NewGuid();
                var milliseconds = SystemClock.Instance.GetCurrentInstant().ToUnixTimeMilliseconds();
                Created = Instant.FromUnixTimeMilliseconds(milliseconds).InZone(provider.GetSystemDefault());
                CreatedDateTime = Created.LocalDateTime;
                CreatedDate = CreatedDateTime.Date;
                CreatedTime = CreatedDateTime.TimeOfDay;
                CreatedOffsetDateTime = Created.ToOffsetDateTime();
                CreatedOffsetTime = CreatedOffsetDateTime.ToOffsetTime();

                Duration = Duration.FromHours(29.5);

                var b = new PeriodBuilder
                {
                    Years = 1,
                    Months = 10,
                    Weeks = 3,
                    Days = 4,
                    Hours = 25,
                    Minutes = 65,
                    Seconds = 100,
                    Milliseconds = 4,
                    Nanoseconds = 3,
                    Ticks = 3
                };

                Period = b.Build();
            }
            public Guid Id { get; protected set; }
            public ZonedDateTime Created { get; protected set; }

            public LocalDateTime CreatedDateTime { get; protected set; }
            public LocalDate CreatedDate { get; protected set; }

            public LocalTime CreatedTime { get; protected set; }

            public OffsetDateTime CreatedOffsetDateTime { get; protected set; }

            public OffsetTime CreatedOffsetTime { get; protected set; }

            public Period Period { get; protected set; }

            public Duration Duration { get; protected set; }
        }
    }
}
