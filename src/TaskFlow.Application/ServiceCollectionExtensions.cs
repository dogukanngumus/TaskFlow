using Microsoft.Extensions.DependencyInjection;
using TaskFlow.Application.Commands.CreateTaskCommand;
using TaskFlow.Application.Commands.DeleteTaskCommand;
using TaskFlow.Application.Commands.UpdateTaskCommand;
using TaskFlow.Application.Commands.Validators;
using TaskFlow.Application.Dispatchers;
using TaskFlow.Application.Queries.GetTasks;
using TaskFlow.Application.Queries.GetTasksById;
using FluentValidation;
using TaskFlow.Application.Behaviours;

namespace TaskFlow.Application;

public static class ServiceCollectionExtensions
{
    public static IServiceCollection AddApplicationServices(this IServiceCollection services)
    {
        services.AddScoped<ICommandDispatcher, CommandDispatcher>();
        services.AddScoped<ICommandHandler<CreateTaskCommand, Guid>, CreateTaskCommandHandler>();
        services.AddScoped<ICommandHandler<UpdateTaskCommand, bool>, UpdateTaskCommandHandler>();
        services.AddScoped<ICommandHandler<DeleteTaskCommand, bool>, DeleteTaskCommandHandler>();
        

        services.AddScoped<IQueryDispatcher, QueryDispatcher>();
        services.AddScoped<IQueryHandler<GetTasksQuery, IEnumerable<TaskDto>>, GetTasksQueryHandler>();
        services.AddScoped<IQueryHandler<GetTaskByIdQuery, TaskDto?>, GetTaskByIdQueryHandler>();

        services.AddScoped(typeof(IPipelineBehavior<,>), typeof(ValidationBehavior<,>));
        services.AddScoped(typeof(IPipelineBehavior<,>), typeof(LoggingBehavior<,>));
        services.AddValidatorsFromAssemblyContaining<CreateTaskCommandValidator>();
        return services;
    }
}
