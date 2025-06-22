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

        dynamic? handler = _serviceProvider.GetService(handlerType);

        if (handler == null)
        {
            throw new InvalidOperationException($"Handler for command {command.GetType().Name} not found.");
        }

        return await handler.Handle((dynamic)command, cancellationToken);
    }

    public async Task Dispatch(ICommand command, CancellationToken cancellationToken = default)
    {
        var handlerType = typeof(ICommandHandler<>).MakeGenericType(command.GetType());

        dynamic? handler = _serviceProvider.GetService(handlerType);
        
        if (handler == null)
        {
            throw new InvalidOperationException($"Handler for command {command.GetType().Name} not found.");
        }

        await handler.Handle((dynamic)command, cancellationToken);
    }
}
