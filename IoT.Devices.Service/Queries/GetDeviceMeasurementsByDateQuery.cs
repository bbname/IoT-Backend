using IoT.Devices.Service.DTOs;
using IoT.Devices.Service.Infrastructure.CQRS;
using System;

namespace IoT.Devices.Service.Queries
{
    public class GetDeviceMeasurementsByDateQuery : IQuery<DeviceMeasurementsDTO>
    {
        public GetDeviceMeasurementsByDateQuery(string deviceId, DateTime? measurementDate)
        {
            DeviceId = deviceId;
            MeasurementDate = measurementDate;
        }

        public string DeviceId { get; }
        public DateTime? MeasurementDate { get; }
    }
}
