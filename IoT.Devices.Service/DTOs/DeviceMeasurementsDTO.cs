using System.Collections.Generic;

namespace IoT.Devices.Service.DTOs
{
    public class DeviceMeasurementsDTO
    {
        public IEnumerable<MeasurementDTO> HumiditySensorMeasurements { get; set; }
        public IEnumerable<MeasurementDTO> RainfallSensorMeasurements { get; set; }
        public IEnumerable<MeasurementDTO> TemperatureSensorMeasurements { get; set; }
    }
}
