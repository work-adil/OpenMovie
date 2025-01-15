using FluentValidation;
using MediatR;
using OpenMovie.Application.CQRS.Models.Responses;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OpenMovie.Application.CQRS.Handlers.Pipelines
{
    public sealed class ValidationBehavior<TRequest, TResponse> : IPipelineBehavior<TRequest, TResponse>
            where TRequest : IRequest<TResponse>
            where TResponse : BaseResult, new()
    {
        private readonly IEnumerable<IValidator<TRequest>> _validators;
        public ValidationBehavior(IEnumerable<IValidator<TRequest>> validators) => _validators = validators;
        public async Task<TResponse> Handle(TRequest request, RequestHandlerDelegate<TResponse> next, CancellationToken cancellationToken)
        {
            if (!_validators.Any())
            {
                return await next();
            }
            var context = new ValidationContext<TRequest>(request);
            var errors = _validators
                .Select(x => x.Validate(context))
                .SelectMany(x => x.Errors)
                .Where(x => x != null);

            if (errors.Any())
            {
                var response = new TResponse() { ResponseStatusCode = System.Net.HttpStatusCode.BadRequest };
                response.Errors = new List<string>(errors.Select(x => x.ErrorMessage).Distinct());
                return response;
            }

            return await next();
        }
    }
}
