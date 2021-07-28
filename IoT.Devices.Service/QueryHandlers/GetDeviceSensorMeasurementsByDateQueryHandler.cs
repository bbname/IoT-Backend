using AutoMapper;
using IoT.Devices.Service.AzureBlobStorage;
using IoT.Devices.Service.CsvHelper;
using IoT.Devices.Service.DTOs;
using IoT.Devices.Service.Infrastructure.CQRS;
using IoT.Devices.Service.Queries;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace IoT.Devices.Service.QueryHandlers
{
    public class GetDeviceSensorMeasurementsByDateQueryHandler : IQueryHandler<GetDeviceSensorMeasurementsByDateQuery, IEnumerable<MeasurementDTO>>
    {
        private readonly IDevicesBlobStorageService _devicesBlobStorageService;
        private readonly IMapper _mapper;
        public GetDeviceSensorMeasurementsByDateQueryHandler(IDevicesBlobStorageService devicesBlobStorageService, IMapper mapper)
        {
            _devicesBlobStorageService = devicesBlobStorageService;
            _mapper = mapper;
        }

        public async Task<IEnumerable<MeasurementDTO>> Handle(GetDeviceSensorMeasurementsByDateQuery request, CancellationToken cancellationToken)
        {
            var getSensorMeasurementFileStreamTask = _devicesBlobStorageService.GetSensorMeasurementFileAsync(
                request.DeviceId, request.SensorType, request.MeasurementDate.Value);
            var result = getSensorMeasurementFileStreamTask.ContinueWith((getCsvFileStreamTask) =>
            {
                using (var measurementCsvReader = new MeasurementCsvReader(getCsvFileStreamTask.Result))
                {
                    var csvRecords = measurementCsvReader.GetMeasurementCsvModels();
                    return _mapper.Map<IEnumerable<MeasurementDTO>>(csvRecords);
                }
            }, TaskContinuationOptions.OnlyOnRanToCompletion);

            return await result;
        }
    }
}
