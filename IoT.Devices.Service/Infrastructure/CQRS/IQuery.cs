using MediatR;

namespace IoT.Devices.Service.Infrastructure.CQRS
{
    public interface IQuery<out TResult> : IRequest<TResult>
    {
    }
}
