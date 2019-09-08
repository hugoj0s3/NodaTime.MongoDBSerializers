# NodaTime.MongoDBSerializers
![image of saved document](https://github.com/hugoj0s3/NodaTime.MongoDBSerializers/blob/master/datasaved.png)

## Getting started
### Registring all supported serializers
```CSharp
 IDateTimeZoneProvider provider = //Set a provider here
 NodaTimeSerializersRegister.RegisterAll(provider);
```
### Registring one by one serializers
```CSharp
  IDateTimeZoneProvider provider = DateTimeZoneProviders.Tzdb;
  ZonedDateTimeSerializer.RegisterSerializer(provider);

  LocalDateTimeSerializer.RegisterSerializer();
  LocalDateSerializer.RegisterSerializer();
  LocalTimeSerializer.RegisterSerializer();

  OffSetDatetimeSerializer.RegisterSerializer();
  OffSetTimeSerializer.RegisterSerializer();
  OffSetDateSerializer.RegisterSerializer();

  PeriodSerializer.RegisterSerializer();
  DurationSerializer.RegisterSerializer();
```
## Types supported
* ZonedDateTime 
* LocalDateTime
* LocalDate
* LocalTime 
* OffsetDateTime 
* OffsetTime 
* Period
* Duration

## Limitation
For the types ZonedDateTime, LocalDateTime, LocalTime, OffsetDateTime, OffsetTime it serialize using the unix milliseconds so it will cutt off the nanoseconds. If you have a scenario where you need to save the current time and get it back from a mongo db and compare both. Use the milliseconds like this:

```CSharp
 var milliseconds = SystemClock.Instance.GetCurrentInstant().ToUnixTimeMilliseconds();
 ZonedTDateTime now = Instant.FromUnixTimeMilliseconds(milliseconds).InUtc();
```