using FluentValidation;
using MediatR;
using RoseApp.Application.Behaviors;

namespace RoseApp.API.Extensions;

public static class ApplicationServiceExtensions
{
    public static IServiceCollection AddApplicationServices(
        this IServiceCollection services)
    {
        var applicationAssembly =
            typeof(Application.Contracts.IDbContext).Assembly;

        services.AddMediatR(cfg =>
            cfg.RegisterServicesFromAssembly(applicationAssembly));

        services.AddAutoMapper(applicationAssembly);

        services.AddValidatorsFromAssembly(applicationAssembly);

        services.AddTransient(
            typeof(IPipelineBehavior<,>),
            typeof(ValidationBehavior<,>));

        return services;
    }
}