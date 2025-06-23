using TaskFlow.Application.Dispatchers;
using TaskFlow.Application.Interfaces;
using TaskFlow.Domain.TaskAggregate;

namespace TaskFlow.Application.Commands.CreateTaskCommand;

public class CreateTaskCommandHandler : ICommandHandler<CreateTaskCommand, Guid>
{
    private readonly ITaskItemRepository _repository;

    public CreateTaskCommandHandler(ITaskItemRepository repository)
    {
        _repository = repository;
    }

    public async Task<Guid> Handle(CreateTaskCommand command, CancellationToken cancellationToken = default)
    {
        var task = TaskItem.Create(command.Title, command.Description, command.DueDate, command.UserId);

        await _repository.AddAsync(task, cancellationToken);
        await _repository.SaveChangesAsync(cancellationToken);
        return task.Id;
    }
}
