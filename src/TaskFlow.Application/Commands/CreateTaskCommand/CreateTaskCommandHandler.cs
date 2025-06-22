using TaskFlow.Application.Dispatchers;
using TaskFlow.Application.Interfaces;
using TaskFlow.Domain.TaskAggregate;
using TaskFlow.Domain.TaskAggregate.ValueObjects;

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
        var title = new Title(command.Title);

        var task = TaskItem.Create(
            title,
            command.Description,
            command.DueDate,
            command.UserId
        );

        await _repository.AddAsync(task, cancellationToken);

        return task.Id;
    }
}
