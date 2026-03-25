using MediatR;

namespace BuildingBlocks.Concerns.CQRS
{
    public interface IQuery<out TResponse> : IRequest<TResponse> where TResponse : notnull
    {
    }
}
