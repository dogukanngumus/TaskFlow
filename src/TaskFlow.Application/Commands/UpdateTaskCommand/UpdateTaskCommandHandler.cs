using TaskFlow.Application.Dispatchers;
using TaskFlow.Application.Interfaces;

namespace TaskFlow.Application.Commands.UpdateTaskCommand;

public sealed class UpdateTaskCommandHandler : ICommandHandler<UpdateTaskCommand, bool>
{
    private readonly ITaskItemRepository _repository;

    public UpdateTaskCommandHandler(ITaskItemRepository repository)
    {
        _repository = repository;
    }
    public async Task<bool> Handle(UpdateTaskCommand command, CancellationToken cancellationToken = default)
    {        
        var task = await _repository.GetByIdAsync(command.TaskId, cancellationToken);
        if (task == null)
        {
            return false; // ya da exception
        }

        task.Update(command.Title, command.Description, command.DueDate);

        await _repository.UpdateAsync(task);
        await _repository.SaveChangesAsync(cancellationToken);
        return true;
    }
}
