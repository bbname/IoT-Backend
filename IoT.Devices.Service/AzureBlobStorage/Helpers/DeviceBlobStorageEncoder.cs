using IoT.Devices.Service.Common.DateHelpers;
using IoT.Devices.Service.Common.EnumHelpers;
using IoT.Devices.Service.DTOs.Enum;
using System;

namespace IoT.Devices.Service.AzureBlobStorage.Helpers
{
    public static class DeviceBlobStorageEncoder
    {
        public static string EncodeTemporaryBlobFileName(string deviceId, SensorType sensorType, DateTime measurementDate)
        {
            var parsedSensorType = SensorTypeEnumHelper.ParseEnum(sensorType);
            var formattedDate = DeviceBlobStorageDateFormatter.Format(measurementDate);

            return $"{deviceId}/{parsedSensorType}/{formattedDate}.csv";
        }

        public static string EncodeHistoricalBlobFileName(string deviceId, SensorType sensorType)
        {
            var parsedSensorType = SensorTypeEnumHelper.ParseEnum(sensorType);

            return $"{deviceId}/{parsedSensorType}/historical.zip";
        }

        public static string EncodeTemporaryFileNameInsideHistoricalBlobFile(DateTime measurementDate)
        {
            var formattedDate = DeviceBlobStorageDateFormatter.Format(measurementDate);

            return $"{formattedDate}/.csv";
        }
    }
}
