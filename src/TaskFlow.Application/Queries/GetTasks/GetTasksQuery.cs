using System;
using TaskFlow.Application.Dispatchers;

namespace TaskFlow.Application.Queries.GetTasks;

public sealed class GetTasksQuery : IQuery<IEnumerable<TaskDto>>
{
}

public sealed class TaskDto
{
    public Guid Id { get; init; }
    public string Title { get; init; } = default!;
    public string? Description { get; init; }
    public DateTime? DueDate { get; init; }
}