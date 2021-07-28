using AutoMapper;
using IoT.Devices.Service.AzureBlobStorage;
using IoT.Devices.Service.CsvHelper;
using IoT.Devices.Service.DTOs;
using IoT.Devices.Service.DTOs.Enum;
using IoT.Devices.Service.Infrastructure.CQRS;
using IoT.Devices.Service.Queries;
using System;
using System.Collections.Generic;
using System.IO;
using System.Threading;
using System.Threading.Tasks;

namespace IoT.Devices.Service.QueryHandlers
{
    public class GetDeviceMeasurementsByDateQueryHandler : IQueryHandler<GetDeviceMeasurementsByDateQuery, DeviceMeasurementsDTO>
    {
        private readonly IDevicesBlobStorageService _devicesBlobStorageService;
        private readonly IMapper _mapper;

        public GetDeviceMeasurementsByDateQueryHandler(IDevicesBlobStorageService devicesBlobStorageService, IMapper mapper)
        {
            _devicesBlobStorageService = devicesBlobStorageService;
            _mapper = mapper;
        }
        public async Task<DeviceMeasurementsDTO> Handle(GetDeviceMeasurementsByDateQuery request, CancellationToken cancellationToken)
        {
            var humidityCsvFileStreamTask = GetMeasurementsAsync(request.DeviceId, SensorType.Humidity, request.MeasurementDate.Value);
            var rainfallCsvFileStreamTask = GetMeasurementsAsync(request.DeviceId, SensorType.Rainfall, request.MeasurementDate.Value);
            var temperatureCsvFileStreamTask = GetMeasurementsAsync(request.DeviceId, SensorType.Temperature, request.MeasurementDate.Value);
            var deviceMeasurementsDTO = new DeviceMeasurementsDTO()
            {
                HumiditySensorMeasurements = await humidityCsvFileStreamTask,
                RainfallSensorMeasurements = await rainfallCsvFileStreamTask,
                TemperatureSensorMeasurements = await temperatureCsvFileStreamTask
            };

            return deviceMeasurementsDTO;
        }

        private async Task<IEnumerable<MeasurementDTO>> GetMeasurementsAsync(string deviceId, SensorType sensorType, DateTime measurementDate)
        {
            var getSensorMeasurementFileStreamTask = _devicesBlobStorageService.GetSensorMeasurementFileAsync(
                deviceId, sensorType, measurementDate);

            return await getSensorMeasurementFileStreamTask.ContinueWith((getCsvFileStreamTask) => {
                return GetMeasurementsFromCsvFileStream(getCsvFileStreamTask.Result);
            }, TaskContinuationOptions.OnlyOnRanToCompletion);
        }

        private IEnumerable<MeasurementDTO> GetMeasurementsFromCsvFileStream(MemoryStream measurementCsvFileStream)
        {
            using (var measurementCsvReader = new MeasurementCsvReader(measurementCsvFileStream))
            {
                var csvRecords = measurementCsvReader.GetMeasurementCsvModels();
                
                return _mapper.Map<IEnumerable<MeasurementDTO>>(csvRecords);
            }
        }
    }
}
