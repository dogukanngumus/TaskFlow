using Microsoft.Extensions.DependencyInjection;
using TaskFlow.Application.Commands.CreateTaskCommand;
using TaskFlow.Application.Commands.DeleteTaskCommand;
using TaskFlow.Application.Commands.UpdateTaskCommand;
using TaskFlow.Application.Dispatchers;

namespace TaskFlow.Application;

public static class ServiceCollectionExtensions
{
    public static IServiceCollection AddApplicationServices(this IServiceCollection services)
    {
        services.AddScoped<ICommandDispatcher, CommandDispatcher>();
        services.AddScoped<ICommandHandler<CreateTaskCommand, Guid>, CreateTaskCommandHandler>();
        services.AddScoped<ICommandHandler<UpdateTaskCommand, bool>, UpdateTaskCommandHandler>();
        services.AddScoped<ICommandHandler<DeleteTaskCommand, bool>, DeleteTaskCommandHandler>();
        
        return services;
    }
}
