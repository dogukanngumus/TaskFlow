using TaskFlow.Application.Dispatchers;
using TaskFlow.Application.Queries.GetTasks;

namespace TaskFlow.Application.Queries.GetTasksById;

public sealed class GetTaskByIdQuery : IQuery<TaskDto?>
{
    public Guid TaskId { get; }

    public GetTaskByIdQuery(Guid taskId)
    {
        TaskId = taskId;
    }
}