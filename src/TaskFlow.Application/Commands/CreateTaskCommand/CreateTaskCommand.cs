using TaskFlow.Application.Dispatchers;

namespace TaskFlow.Application.Commands.CreateTaskCommand;

public sealed class CreateTaskCommand: ICommand<Guid>
{
    public string Title { get; init; }
    public string Description { get; init; }
    public DateTime DueDate { get; init; }
    public Guid UserId { get; init; }

    public CreateTaskCommand(string title, string description, DateTime dueDate, Guid userId)
    {
        Title = title;
        Description = description;
        DueDate = dueDate;
        UserId= userId;
    }
}
