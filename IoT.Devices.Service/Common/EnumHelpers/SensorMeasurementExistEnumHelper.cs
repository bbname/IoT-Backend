using IoT.Devices.Service.AzureBlobStorage.Constants;
using IoT.Devices.Service.AzureBlobStorage.Enums;
using System;

namespace IoT.Devices.Service.Common.EnumHelpers
{
    public static class SensorMeasurementExistEnumHelper
    {
        public static string ParseEnum(SensorMeasurementExist sensorMeasurementExist)
        {
            return sensorMeasurementExist switch
            {
                SensorMeasurementExist.None => SensorMeasurementExistConstants.None,
                SensorMeasurementExist.TemporaryFile => SensorMeasurementExistConstants.TemporaryFile,
                SensorMeasurementExist.HistoricalFile => SensorMeasurementExistConstants.HistoricalFile,
                _ => throw new ArgumentOutOfRangeException(
                    $"There is no implementation of {Enum.GetName(typeof(SensorMeasurementExist), sensorMeasurementExist)} as sensor measurement exist."),
            };
        }

        public static SensorMeasurementExist ParseString(string sensorMeasurementExist)
        {
            return sensorMeasurementExist switch
            {
                SensorMeasurementExistConstants.None => SensorMeasurementExist.None,
                SensorMeasurementExistConstants.TemporaryFile => SensorMeasurementExist.TemporaryFile,
                SensorMeasurementExistConstants.HistoricalFile => SensorMeasurementExist.HistoricalFile,
                _ => throw new ArgumentOutOfRangeException(
                    $"There is no implementation of {sensorMeasurementExist} as sensor measurement exist."),
            };
        }
    }
}
