namespace TaskFlow.Application.Dispatchers;

public interface ICommandDispatcher
{
    Task<TResult> Dispatch<TResult>(ICommand<TResult> command, CancellationToken cancellationToken = default);
    Task Dispatch(ICommand command, CancellationToken cancellationToken = default);
}
