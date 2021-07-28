using IoT.Devices.Service.Common.DateHelpers;
using IoT.Devices.Service.Common.EnumHelpers;
using IoT.Devices.Service.DTOs.Enum;
using System;

namespace IoT.Devices.Service.Infrastructure.Cache
{
    public static class DeviceBlobStorageServiceCacheKeys
    {
        public static string GetSensorMeasurementsKey(string deviceId, SensorType sensorType, DateTime measurementDate)
        {
            return $"Device-{deviceId}-Sensor-{SensorTypeEnumHelper.ParseEnum(sensorType)}-MeasurementDate-{DeviceBlobStorageDateFormatter.Format(measurementDate)}";
        }
    }
}
