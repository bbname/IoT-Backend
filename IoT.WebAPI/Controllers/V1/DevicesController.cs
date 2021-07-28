using IoT.Devices.Service.DTOs;
using IoT.Devices.Service.Queries;
using IoT.WebAPI.Infrastructure.ErrorHandling.Responses;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace IoT.WebAPI.Controllers.V1
{
    [ApiVersion("1.0")]
    [Route("api/v{version:apiVersion}/devices")]
    [ApiController]
    public class DevicesController : ControllerBase
    {
        private readonly IMediator _mediator;

        public DevicesController(IMediator mediator)
        {
            this._mediator = mediator;
        }

        /// <summary>
        /// Get all measurement types of the device from specified date.
        /// </summary>
        /// <param name="deviceId">Device identifier</param> 
        /// <param name="measurementDate">Measurement date</param> 
        /// <returns>All measurement types of the device from specified date.</returns>
        /// <response code="200">Measurements successfully fetched.</response>
        /// <response code="400">The filter is invalid.</response>
        /// <response code="404">Device or measurements not found.</response>
        [HttpGet]
        [Route("{deviceId}/measurements/{measurementDate}")]
        [Produces("application/json")]
        [ProducesResponseType(typeof(DeviceMeasurementsDTO), 200)]
        [ProducesResponseType(typeof(ValidationErrorResponse), 400)]
        [ProducesResponseType(typeof(ResourceNotFoundErrorResponse), 404)]
        public async Task<DeviceMeasurementsDTO> GetDeviceMeasurements(string deviceId, DateTime? measurementDate)
        {
            var result = await _mediator.Send(new GetDeviceMeasurementsByDateQuery(deviceId, measurementDate));
            
            return result;
        }

        /// <summary>
        /// Get all measurement types of the sensor from specified date and sensor type.
        /// </summary>
        /// <param name="deviceId">Device identifier</param> 
        /// <param name="measurementDate">Measurement date</param> 
        /// <param name="sensorType">Sensor type</param> 
        /// <returns>All measurement types of the sensor from specified date and device.</returns>
        /// <response code="200">Measurements successfully fetched.</response>
        /// <response code="400">The filter is invalid.</response>
        /// <response code="404">Device or sensor or measurement not found.</response>
        [HttpGet]
        [Route("{deviceId}/measurements/{measurementDate}/sensorTypes/{sensorType}")]
        [Produces("application/json")]
        [ProducesResponseType(typeof(IEnumerable<MeasurementDTO>), 200)]
        [ProducesResponseType(typeof(ValidationErrorResponse), 400)]
        [ProducesResponseType(typeof(ResourceNotFoundErrorResponse), 404)]
        public async Task<IEnumerable<MeasurementDTO>> GetSensorTypeMeasurements(string deviceId, DateTime? measurementDate, string sensorType)
        {
            var result = await _mediator.Send(new GetDeviceSensorMeasurementsByDateQuery(deviceId, measurementDate, sensorType));

            return result;
        }
    }
}
