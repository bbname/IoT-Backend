using System;

namespace IoT.Devices.Service.Infrastructure.Cache
{
    public static class DeviceBlobStorageServiceCacheTimes
    {
        public static TimeSpan GenerateCacheTime()
        {
            var dt = DateTime.UtcNow;
            var nextDay = dt.AddDays(1);
            var nextDayStart = new DateTime(nextDay.Year, nextDay.Month, nextDay.Day, 0, 0, 0, 0, DateTimeKind.Utc);

            return nextDayStart - dt;
        }
    }
}
