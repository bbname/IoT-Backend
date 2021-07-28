using AutoMapper;
using Azure.Storage.Blobs;
using IoT.Devices.Service.AzureBlobStorage.Enums;
using IoT.Devices.Service.AzureBlobStorage.Helpers;
using IoT.Devices.Service.DTOs.Enum;
using IoT.Devices.Service.Exceptions;
using IoT.Devices.Service.Infrastructure.AppSettings;
using IoT.Devices.Service.Infrastructure.Cache;
using IoT.Devices.Service.ZipHelper;
using LazyCache;
using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace IoT.Devices.Service.AzureBlobStorage
{
    public class DevicesBlobStorageService : IDevicesBlobStorageService
    {
        private readonly BlobServiceClient _blobServiceClient;
        private readonly IOptions<AzureBlobStorageAppSettings> _azureBlobStorageAppSettings;
        private readonly IDevicesBlobStorageServiceCache _cache;

        public DevicesBlobStorageService(BlobServiceClient blobServiceClient,
            IOptions<AzureBlobStorageAppSettings> azureBlobStorageAppSettings,
            IDevicesBlobStorageServiceCache cache)
        {
            _blobServiceClient = blobServiceClient;
            _azureBlobStorageAppSettings = azureBlobStorageAppSettings;
            _cache = cache;
        }

        public async Task<MemoryStream> GetSensorMeasurementFileAsync(string deviceId, SensorType sensorType, DateTime measurementDate)
        {
            var isSensorMeasurementCached = _cache.IsSensorMeasurementCached(deviceId, sensorType, measurementDate, out var sensorMeasurementCacheModel);
            MemoryStream csvFileStream;
            var temporaryFileName = DeviceBlobStorageEncoder.EncodeTemporaryBlobFileName(deviceId, sensorType, measurementDate);

            if (isSensorMeasurementCached && sensorMeasurementCacheModel.SensorMeasurementExist == SensorMeasurementExist.None)
            {
                throw new ResourceNotFoundException(temporaryFileName.Replace(".csv", ""), "Device measurements");
            }
            else if (isSensorMeasurementCached && sensorMeasurementCacheModel.SensorMeasurementData != null)
            {
                var memoryStream = new MemoryStream();
                memoryStream.Write(sensorMeasurementCacheModel.SensorMeasurementData, 0, sensorMeasurementCacheModel.SensorMeasurementData.Length);
                csvFileStream = memoryStream;
            }
            else
            {
                var container = _blobServiceClient.GetBlobContainerClient(_azureBlobStorageAppSettings.Value.ContainerName);
                var temporaryFileBlobClient = container.GetBlobClient(temporaryFileName);
                var isTemporaryFileExistsResponse = await temporaryFileBlobClient.ExistsAsync();

                if (isTemporaryFileExistsResponse.Value)
                {
                    using (var memoryStream = new MemoryStream())
                    {
                        await temporaryFileBlobClient.DownloadToAsync(memoryStream);
                        csvFileStream = memoryStream;
                    }
                    _cache.CreateCacheSensorMeasurement(deviceId, sensorType, measurementDate, SensorMeasurementExist.TemporaryFile, csvFileStream);
                }
                else
                {
                    var historicalFileName = DeviceBlobStorageEncoder.EncodeHistoricalBlobFileName(deviceId, sensorType);
                    var historicalFileBlobClient = container.GetBlobClient(historicalFileName);
                    var isHistoricalFileExistsResponse = await historicalFileBlobClient.ExistsAsync();

                    if (isHistoricalFileExistsResponse.Value)
                    {
                        using (var memoryStream = new MemoryStream())
                        {
                            await historicalFileBlobClient.DownloadToAsync(memoryStream);
                            using (var measurementsArchiveReader = new MeasurementsArchiveReader(memoryStream))
                            {
                                measurementsArchiveReader.ReadMeasurementStream(measurementDate);

                                if (!isSensorMeasurementCached)
                                {
                                    var measurementDateFileNamesToSkip = new List<string>()
                                    {
                                        DeviceBlobStorageEncoder.EncodeTemporaryFileNameInsideHistoricalBlobFile(measurementDate)
                                    };
                                    var distinctMeasurementDateFileNames = measurementsArchiveReader.MeasurementDateFileNames.Except(
                                        measurementDateFileNamesToSkip);
                                    _cache.CreateCacheSensorMeasurementsFromHistoricalFile(deviceId, sensorType, distinctMeasurementDateFileNames);
                                }
                                csvFileStream = measurementsArchiveReader.MeasurementCsvStream;
                            }

                            _cache.UpdateCacheSensorMeasurement(deviceId, sensorType, measurementDate, SensorMeasurementExist.HistoricalFile, csvFileStream);
                        }
                    }
                    else
                    {
                        _cache.CreateCacheSensorMeasurement(deviceId, sensorType, measurementDate, SensorMeasurementExist.None, null);

                        throw new ResourceNotFoundException(temporaryFileName.Replace(".csv", ""), "Device measurements");
                    }
                }
            }

            return csvFileStream;
        }

        //private bool IsSensorMeasurementCached(string deviceId, SensorType sensorType, DateTime measurementDate,
        //    out SensorMeasurementCacheModel sensorMeasurementCacheModel)
        //{
        //    sensorMeasurementCacheModel = null;
        //    var isCached = _cache.TryGetValue<SensorMeasurementCacheModel>(
        //        DeviceBlobStorageServiceCacheKeys.GetSensorMeasurementsKey(deviceId, sensorType, measurementDate),
        //        out var sensorMeasurementCacheModelBoxed);

        //    if (isCached)
        //    {
        //        sensorMeasurementCacheModel = (SensorMeasurementCacheModel)sensorMeasurementCacheModelBoxed;  
        //    }

        //    return isCached;
        //}

        //private void CreateCacheSensorMeasurement(string deviceId, SensorType sensorType, DateTime measurementDate,
        //    SensorMeasurementExist sensorMeasurementExist, MemoryStream sensorMeasurementStream)
        //{
        //    var cacheKey = DeviceBlobStorageServiceCacheKeys.GetSensorMeasurementsKey(deviceId, sensorType, measurementDate);
        //    var sensorMeasurementCacheModel = new SensorMeasurementCacheModel(sensorMeasurementExist, sensorMeasurementStream?.ToArray());
        //    _cache.Add(cacheKey, sensorMeasurementCacheModel, DeviceBlobStorageServiceCacheTimes.GenerateCacheTime());
        //}

        //private void UpdateCacheSensorMeasurement(string deviceId, SensorType sensorType, DateTime measurementDate,
        //    SensorMeasurementExist sensorMeasurementExist, MemoryStream sensorMeasurementStream)
        //{
        //    var cacheKey = DeviceBlobStorageServiceCacheKeys.GetSensorMeasurementsKey(deviceId, sensorType, measurementDate);
        //    var isAlreadyExistsInCache = _cache.TryGetValue<SensorMeasurementCacheModel>(cacheKey, out var sensorMeasurementCacheModelBoxed);

        //    if (isAlreadyExistsInCache)
        //    {
        //        _cache.Remove(cacheKey);
        //    }

        //    CreateCacheSensorMeasurement(deviceId, sensorType, measurementDate, sensorMeasurementExist, sensorMeasurementStream);
        //}

        //private void CreateCacheSensorMeasurementsFromHistoricalFile(string deviceId, SensorType sensorType, 
        //    IEnumerable<string> measurementDateFileNames)
        //{
        //    foreach (var measurementDateFileName in measurementDateFileNames)
        //    {
        //        CreateCacheSensorMeasurement(
        //            deviceId,
        //            sensorType,
        //            DeviceBlobStorageDecoder.DecodeMeasurementDateFromTemporaryFileNameFromHistoricalBlobFile(measurementDateFileName),
        //            SensorMeasurementExist.HistoricalFile,
        //            null);
        //    }
        //}
    }
}
