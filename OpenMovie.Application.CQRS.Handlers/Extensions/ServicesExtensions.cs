using FluentValidation;
using MediatR;
using Microsoft.Extensions.DependencyInjection;
using OpenMovie.Application.CQRS.Handlers.Handlers;
using OpenMovie.Application.CQRS.Handlers.Pipelines;
using OpenMovie.Application.CQRS.Handlers.Validators;

namespace OpenMovie.Application.CQRS.Handlers.Extensions
{
    public static class ServiceExtensions
    {
        /// <summary>
        /// Adds MediatRServices.
        /// </summary>
        /// <param name="services">Services collection.</param>
        public static IServiceCollection AddMediatRServices(this IServiceCollection services)
        {
            return services
                .AddMediatR(x => x.RegisterServicesFromAssemblyContaining<GetMoviesQueryHandler>())
                .AddTransient(typeof(IPipelineBehavior<,>), typeof(ValidationBehavior<,>))
                .AddValidatorsFromAssembly(typeof(GetMoviesQueryValidator).Assembly);
        }
    }
}
