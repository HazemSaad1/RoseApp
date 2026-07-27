using FluentValidation;
using MediatR;
using System;
using System.Collections.Generic;
using System.Text;

namespace RoseApp.Application.Behaviors
{

    public class ValidationBehavior<TRequest, TResponse>
        : IPipelineBehavior<TRequest, TResponse>
        where TRequest : IRequest<TResponse>
    {
        private readonly IEnumerable<IValidator<TRequest>> _validators;

        public ValidationBehavior(IEnumerable<IValidator<TRequest>> validators)
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

            var context = new ValidationContext<TRequest>(request);

            var errors = _validators
                .Select(v => v.Validate(context))
                .SelectMany(r => r.Errors)
                .Where(f => f != null)
                .GroupBy(
                    f => f.PropertyName,
                    f => f.ErrorMessage,
                    (key, messages) => new { Key = key, Messages = messages.ToArray() })
                .ToDictionary(e => e.Key, e => e.Messages);

            if (errors.Any())
                throw new Exceptions.ValidationException(errors);

            return await next();
        }
    }
}
