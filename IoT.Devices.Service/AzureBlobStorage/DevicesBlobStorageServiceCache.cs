using IoT.Devices.Service.AzureBlobStorage.Enums;
using IoT.Devices.Service.AzureBlobStorage.Helpers;
using IoT.Devices.Service.DTOs.Enum;
using IoT.Devices.Service.Infrastructure.Cache;
using LazyCache;
using System;
using System.Collections.Generic;
using System.IO;

namespace IoT.Devices.Service.AzureBlobStorage
{
    public class DevicesBlobStorageServiceCache : IDevicesBlobStorageServiceCache
    {
        private readonly IAppCache _cache;

        public DevicesBlobStorageServiceCache(IAppCache cache)
        {
            _cache = cache;
        }

        public void CreateCacheSensorMeasurement(string deviceId, SensorType sensorType, DateTime measurementDate, SensorMeasurementExist sensorMeasurementExist, MemoryStream sensorMeasurementStream)
        {
            var cacheKey = DeviceBlobStorageServiceCacheKeys.GetSensorMeasurementsKey(deviceId, sensorType, measurementDate);
            var sensorMeasurementCacheModel = new SensorMeasurementCacheModel(sensorMeasurementExist, sensorMeasurementStream?.ToArray());
            _cache.Add(cacheKey, sensorMeasurementCacheModel, DeviceBlobStorageServiceCacheTimes.GenerateCacheTime());
        }

        public void CreateCacheSensorMeasurementsFromHistoricalFile(string deviceId, SensorType sensorType, IEnumerable<string> measurementDateFileNames)
        {
            foreach (var measurementDateFileName in measurementDateFileNames)
            {
                CreateCacheSensorMeasurement(
                    deviceId,
                    sensorType,
                    DeviceBlobStorageDecoder.DecodeMeasurementDateFromTemporaryFileNameFromHistoricalBlobFile(measurementDateFileName),
                    SensorMeasurementExist.HistoricalFile,
                    null);
            }
        }

        public void UpdateCacheSensorMeasurement(string deviceId, SensorType sensorType, DateTime measurementDate, SensorMeasurementExist sensorMeasurementExist, MemoryStream sensorMeasurementStream)
        {
            var cacheKey = DeviceBlobStorageServiceCacheKeys.GetSensorMeasurementsKey(deviceId, sensorType, measurementDate);
            var isAlreadyExistsInCache = _cache.TryGetValue<SensorMeasurementCacheModel>(cacheKey, out var sensorMeasurementCacheModelBoxed);

            if (isAlreadyExistsInCache)
            {
                _cache.Remove(cacheKey);
            }

            CreateCacheSensorMeasurement(deviceId, sensorType, measurementDate, sensorMeasurementExist, sensorMeasurementStream);
        }

        public bool IsSensorMeasurementCached(string deviceId, SensorType sensorType, DateTime measurementDate,
            out SensorMeasurementCacheModel sensorMeasurementCacheModel)
        {
            sensorMeasurementCacheModel = null;
            var isCached = _cache.TryGetValue<SensorMeasurementCacheModel>(
                DeviceBlobStorageServiceCacheKeys.GetSensorMeasurementsKey(deviceId, sensorType, measurementDate),
                out var sensorMeasurementCacheModelBoxed);

            if (isCached)
            {
                sensorMeasurementCacheModel = (SensorMeasurementCacheModel)sensorMeasurementCacheModelBoxed;
            }

            return isCached;
        }
    }
}
