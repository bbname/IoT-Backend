using MediatR;

namespace IoT.Devices.Service.Infrastructure.CQRS
{
    public interface IQueryHandler<in TQuery, TResult> : IRequestHandler<TQuery, TResult>
        where TQuery : IQuery<TResult>
    {
    }
}
