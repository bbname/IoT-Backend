using IoT.Devices.Service.DTOs.Enum;
using System;
using System.IO;
using System.Threading.Tasks;

namespace IoT.Devices.Service.AzureBlobStorage
{
    public interface IDevicesBlobStorageService
    {
        Task<MemoryStream> GetSensorMeasurementFileAsync(string deviceId, SensorType sensorType, DateTime date);
    }
}
