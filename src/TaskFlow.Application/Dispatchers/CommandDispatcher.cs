using Microsoft.Extensions.DependencyInjection;
using TaskFlow.Application.Behaviours;

namespace TaskFlow.Application.Dispatchers;

public class CommandDispatcher : ICommandDispatcher
{
    private readonly IServiceProvider _serviceProvider;

    public CommandDispatcher(IServiceProvider serviceProvider)
    {
        _serviceProvider = serviceProvider;
    }

    public async Task<TResult> Dispatch<TResult>(ICommand<TResult> command, CancellationToken cancellationToken = default)
    {
        var handlerType = typeof(ICommandHandler<,>).MakeGenericType(command.GetType(), typeof(TResult));
        dynamic handler = _serviceProvider.GetService(handlerType);

        if (handler == null)
            throw new InvalidOperationException($"Handler for command {command.GetType().Name} not found.");

        var behaviors = _serviceProvider.GetServices(typeof(IPipelineBehavior<,>)
            .MakeGenericType(command.GetType(), typeof(TResult)))
            .Cast<dynamic>()
            .ToList();

        RequestHandlerDelegate<TResult> handlerDelegate = () => handler.Handle((dynamic)command, cancellationToken);

        foreach (var behavior in behaviors.AsEnumerable().Reverse())
        {
            var next = handlerDelegate;
            handlerDelegate = () => behavior.Handle((dynamic)command, cancellationToken, next);
        }
        return await handlerDelegate();
    }
}