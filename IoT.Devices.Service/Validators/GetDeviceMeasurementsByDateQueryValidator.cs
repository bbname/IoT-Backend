using FluentValidation;
using IoT.Devices.Service.Queries;

namespace IoT.Devices.Service.Validators
{
    public class GetDeviceMeasurementsByDateQueryValidator : AbstractValidator<GetDeviceMeasurementsByDateQuery>
    {
        public GetDeviceMeasurementsByDateQueryValidator()
        {
            RuleFor(d => d.DeviceId).NotEmpty();
            RuleFor(d => d.MeasurementDate).NotNull();
        }
    }
}
