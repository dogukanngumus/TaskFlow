using TaskFlow.Application.Dispatchers;
using TaskFlow.Application.Interfaces;

namespace TaskFlow.Application.Commands.DeleteTaskCommand;

public sealed class DeleteTaskCommandHandler : ICommandHandler<DeleteTaskCommand, bool>
{
    private readonly ITaskItemRepository _repository;

    public DeleteTaskCommandHandler(ITaskItemRepository repository)
    {
        _repository = repository;
    }

    public async Task<bool> Handle(DeleteTaskCommand command, CancellationToken cancellationToken = default)
    {
        
        var task = await _repository.GetByIdAsync(command.TaskId, cancellationToken);
        if (task == null)
        {
            return false; // ya da exception fırlatabilirsin
        }

        await _repository.DeleteAsync(task, cancellationToken);
        return true;
    }
}
