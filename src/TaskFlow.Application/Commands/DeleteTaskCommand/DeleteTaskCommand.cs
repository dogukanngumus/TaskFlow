using TaskFlow.Application.Dispatchers;

namespace TaskFlow.Application.Commands.DeleteTaskCommand;

public sealed class DeleteTaskCommand : ICommand<bool>
{
    public Guid TaskId { get; init; }
    public DeleteTaskCommand(Guid taskId)
    {
        TaskId = taskId;
    }
}
