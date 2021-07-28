using IoT.Devices.Service.AzureBlobStorage.Enums;

namespace IoT.Devices.Service.Infrastructure.Cache
{
    public class SensorMeasurementCacheModel
    {
        public SensorMeasurementCacheModel()
        {

        }

        public SensorMeasurementCacheModel(SensorMeasurementExist sensorMeasurementExist, byte[] sensorMeasurementData)
        {
            SensorMeasurementExist = sensorMeasurementExist;
            SensorMeasurementData = sensorMeasurementData;
        }

        public SensorMeasurementExist SensorMeasurementExist { get; }
        public byte[] SensorMeasurementData { get; }
    }
}
