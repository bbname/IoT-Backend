using IoT.Devices.Service.DTOs.Constants;
using IoT.Devices.Service.DTOs.Enum;
using System;

namespace IoT.Devices.Service.Common.EnumHelpers
{
    public static class SensorTypeEnumHelper
    {
        public static string ParseEnum(SensorType sensorType)
        {
            return sensorType switch
            {
                SensorType.Humidity => SensorTypeConstants.Humidity,
                SensorType.Rainfall => SensorTypeConstants.Rainfall,
                SensorType.Temperature => SensorTypeConstants.Temperature,
                _ => throw new ArgumentOutOfRangeException($"There is no implementation of {Enum.GetName(typeof(SensorType), sensorType)} as sensor type."),
            };
        }

        public static SensorType ParseString(string sensorType)
        {
            return sensorType switch
            {
                SensorTypeConstants.Humidity => SensorType.Humidity,
                SensorTypeConstants.Rainfall => SensorType.Rainfall,
                SensorTypeConstants.Temperature => SensorType.Temperature,
                _ => throw new ArgumentOutOfRangeException($"There is no implementation of {sensorType} as sensor type."),
            };
        }
    }
}
