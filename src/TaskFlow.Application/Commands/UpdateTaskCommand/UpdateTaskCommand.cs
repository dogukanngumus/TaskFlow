using TaskFlow.Application.Dispatchers;

namespace TaskFlow.Application.Commands.UpdateTaskCommand;

public sealed class UpdateTaskCommand : ICommand<bool>
{
    public Guid TaskId { get; init; }
    public string Title { get; init; }
    public string Description { get; init; }
    public DateTime DueDate { get; init; }

    public UpdateTaskCommand(Guid taskId, string title, string description, DateTime dueDate)
    {
        TaskId = taskId;
        Title = title;
        Description = description;
        DueDate = dueDate;
    }
}
