using TaskFlow.Application.Dispatchers;
using TaskFlow.Application.Interfaces;
using TaskFlow.Application.Queries.GetTasks;

namespace TaskFlow.Application.Queries.GetTasksById;

public sealed class GetTaskByIdQueryHandler : IQueryHandler<GetTaskByIdQuery, TaskDto?>
{
    private readonly ITaskItemRepository _repository;

    public GetTaskByIdQueryHandler(ITaskItemRepository repository)
    {
        _repository = repository;
    }

    public async Task<TaskDto?> Handle(GetTaskByIdQuery query, CancellationToken cancellationToken = default)
    {
        var task = await _repository.GetByIdAsync(query.TaskId, cancellationToken);

        if (task is null)
            return null;

        return new TaskDto
        {
            Id = task.Id,
            Title = task.Title.Value,
            Description = task.Description,
            DueDate = task.DueDate
        };
    }
}
