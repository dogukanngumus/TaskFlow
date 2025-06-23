using TaskFlow.Domain.TaskAggregate.Exceptions;
using TaskFlow.Domain.TaskAggregate.ValueObjects;

namespace TaskFlow.Domain.TaskAggregate;

public class TaskItem
{
    public Guid Id { get; private set; }
    public Title Title { get; private set; }
    public string Description { get; private set; }
    public TaskStatus Status { get; private set; }
    public DateTime DueDate { get; private set; }
    public Guid UserId { get; private set; }

    private TaskItem() // For EF Core
    {
        
    }    
    private TaskItem(Guid id, Title title, string description, DateTime dueDate, Guid userId)
    {
        if (dueDate < DateTime.UtcNow)
            throw new InvalidDueDateException("Due date cannot be in the past.");
        Id = id;
        Title = title;
        Description = description;
        DueDate = dueDate;
        UserId = userId;
        Status = TaskStatus.ToDo;
    }

    public static TaskItem Create(string title, string description, DateTime dueDate, Guid userId)
    {
        var validTitle = new Title(title);
        return new TaskItem(Guid.NewGuid(), validTitle, description, dueDate, userId);
    }

    public void Update(string title, string description, DateTime dueDate)
    {
        if (dueDate < DateTime.UtcNow)
            throw new InvalidDueDateException("Due date cannot be in the past.");

        Title = new Title(title);
        Description = description;
        DueDate = dueDate;
    }

    public void MoveToInProgress()
    {
        if (Status == TaskStatus.Done)
            throw new InvalidTaskStatusException("Task is already completed.");
        if (Status == TaskStatus.InProgress)
            return;

        Status = TaskStatus.InProgress;
    }
    
    public void Complete()
    {
        if (Status == TaskStatus.Done)
            throw new InvalidTaskStatusException("Task is already completed.");

        Status = TaskStatus.Done;
    }
}
