﻿using FluentValidation;
using IoT.Devices.Service.Queries;

namespace IoT.Devices.Service.Validators
{
    public class GetDeviceSensorMeasurementsByDateQueryValidator : AbstractValidator<GetDeviceSensorMeasurementsByDateQuery>
    {
        public GetDeviceSensorMeasurementsByDateQueryValidator()
        {
            RuleFor(d => d.DeviceId).NotEmpty();
            RuleFor(d => d.MeasurementDate).NotNull();
        }
    }
}
