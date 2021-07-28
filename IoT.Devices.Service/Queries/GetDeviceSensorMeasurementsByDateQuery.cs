using IoT.Devices.Service.Common.EnumHelpers;
using IoT.Devices.Service.DTOs;
using IoT.Devices.Service.DTOs.Enum;
using IoT.Devices.Service.Infrastructure.CQRS;
using System;
using System.Collections.Generic;

namespace IoT.Devices.Service.Queries
{
    public class GetDeviceSensorMeasurementsByDateQuery : IQuery<IEnumerable<MeasurementDTO>>
    {
        public GetDeviceSensorMeasurementsByDateQuery(string deviceId, DateTime? measurementDate, string sensorType)
        {
            DeviceId = deviceId;
            MeasurementDate = measurementDate;
            SensorType = SensorTypeEnumHelper.ParseString(sensorType);
        }

        public string DeviceId { get; }
        public DateTime? MeasurementDate { get; }
        public SensorType SensorType { get; }
    }
}
