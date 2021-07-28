using IoT.Devices.Service.AzureBlobStorage.Enums;
using IoT.Devices.Service.DTOs.Enum;
using IoT.Devices.Service.Infrastructure.Cache;
using System;
using System.Collections.Generic;
using System.IO;

namespace IoT.Devices.Service.AzureBlobStorage
{
    public interface IDevicesBlobStorageServiceCache
    {
        void CreateCacheSensorMeasurement(string deviceId, SensorType sensorType, DateTime measurementDate,
            SensorMeasurementExist sensorMeasurementExist, MemoryStream sensorMeasurementStream);

        void UpdateCacheSensorMeasurement(string deviceId, SensorType sensorType, DateTime measurementDate,
            SensorMeasurementExist sensorMeasurementExist, MemoryStream sensorMeasurementStream);

        void CreateCacheSensorMeasurementsFromHistoricalFile(string deviceId, SensorType sensorType,
            IEnumerable<string> measurementDateFileNames);

        bool IsSensorMeasurementCached(string deviceId, SensorType sensorType, DateTime measurementDate,
            out SensorMeasurementCacheModel sensorMeasurementCacheModel);
    }
}
