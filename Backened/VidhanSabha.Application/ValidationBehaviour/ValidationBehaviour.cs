// Application/Behaviours/ValidationBehaviour.cs
using FluentValidation;
using MediatR;
using VidhanSabha.Application.Exceptions;
using ValidationException = VidhanSabha.Application.Exceptions.ValidationException;

namespace VidhanSabha.Application.Behaviours
{
    public class ValidationBehaviour<TRequest, TResponse>
        : IPipelineBehavior<TRequest, TResponse>
        where TRequest : IRequest<TResponse>
    {
        private readonly IEnumerable<IValidator<TRequest>> _validators;

        public ValidationBehaviour(IEnumerable<IValidator<TRequest>> validators)
        {
            _validators = validators;
        }   

        public async Task<TResponse> Handle(
            TRequest request,
            RequestHandlerDelegate<TResponse> next,
            CancellationToken cancellationToken)
        {
            if (!_validators.Any())
                return await next();

            // Run all FluentValidation validators
            var context = new ValidationContext<TRequest>(request);

            var failures = _validators
                .Select(v => v.Validate(context))
                .SelectMany(r => r.Errors)
                .Where(f => f != null)
                .ToList();

            if (failures.Any())
            {
                // Group errors by field name
                var errors = failures
                    .GroupBy(f => f.PropertyName)
                    .ToDictionary(
                        k => k.Key,
                        v => v.Select(f => f.ErrorMessage).ToArray()
                    );

                throw new ValidationException(errors);
            }

            return await next();
        }
    }
}