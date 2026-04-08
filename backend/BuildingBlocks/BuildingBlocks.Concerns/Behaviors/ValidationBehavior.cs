using BuildingBlocks.Concerns.CQRS;
using FluentValidation;
using MediatR;

namespace BuildingBlocks.Concerns.Behaviors
{
    public class ValidationBehavior<TRequest, TResponse>
        (IEnumerable<IValidator<TRequest>> validadores)
        : IPipelineBehavior<TRequest, TResponse>
        where TRequest : ICommand<TResponse>
    {
        public async Task<TResponse> Handle(TRequest request, RequestHandlerDelegate<TResponse> next, CancellationToken cancellationToken)
        {
            var context = new ValidationContext<TRequest>(request);

            var resultValidators = await Task.WhenAll(validadores.Select(c => c.ValidateAsync(context, cancellationToken)));

            var failures = resultValidators
                .Where(c => c.Errors.Any())
                .SelectMany(c => c.Errors)
                .ToList();

            if(failures.Any())
            {
                throw new ValidationException(failures);
            }

            return await next();
        }
    }
}
