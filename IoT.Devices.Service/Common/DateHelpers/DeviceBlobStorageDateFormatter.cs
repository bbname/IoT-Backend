using System;

namespace IoT.Devices.Service.Common.DateHelpers
{
    public static class DeviceBlobStorageDateFormatter
    {
        public static string Format(DateTime date)
        {
            return date.ToString("yyyy-MM-dd");
        }
    }
}
