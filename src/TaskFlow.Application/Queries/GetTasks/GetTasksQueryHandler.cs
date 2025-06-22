using TaskFlow.Application.Dispatchers;
using TaskFlow.Application.Interfaces;

namespace TaskFlow.Application.Queries.GetTasks;

public sealed class GetTasksQueryHandler : IQueryHandler<GetTasksQuery, IEnumerable<TaskDto>>
{
    private readonly ITaskItemRepository _repository;

    public GetTasksQueryHandler(ITaskItemRepository repository)
    {
        _repository = repository;
    }

    public async Task<IEnumerable<TaskDto>> Handle(GetTasksQuery query, CancellationToken cancellationToken = default)
    {
        var taskItems = await _repository.GetAllAsync();

        var projected = taskItems.Select(task => new TaskDto
        {
            Id = task.Id,
            Title = task.Title.Value,
            Description = task.Description,
            DueDate = task.DueDate
        });

         return await Task.FromResult(projected.ToList());
    }
}
