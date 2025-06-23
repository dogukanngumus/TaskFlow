using TaskFlow.Application.Commands.CreateTaskCommand;
using TaskFlow.Application.Commands.DeleteTaskCommand;
using TaskFlow.Application.Commands.UpdateTaskCommand;
using TaskFlow.Application.Dispatchers;
using TaskFlow.Application.Interfaces;

namespace TaskFlow.API.Endpoints;

public static class TaskEndpoints
{
    public static void MapTaskEndpoints(this WebApplication app)
    {
        app.MapGet("/tasks", async (ITaskItemRepository repo) =>
        {
            var tasks = await repo.GetAllAsync();
            return Results.Ok(tasks.Select(t => new
            {
                t.Id,
                Title = t.Title.Value,
                t.Description,
                t.DueDate,
                t.Status
            }));
        });

        app.MapGet("/tasks/{id}", async (Guid id, ITaskItemRepository repo) =>
        {
            var task = await repo.GetByIdAsync(id);
            if (task == null) return Results.NotFound();

            return Results.Ok(new
            {
                task.Id,
                Title = task.Title.Value,
                task.Description,
                task.DueDate,
                task.Status
            });
        });

        app.MapPost("/tasks", async (CreateTaskCommand command, ICommandDispatcher dispatcher) =>
        {
            var taskId = await dispatcher.Dispatch(command);
            return Results.Created($"/tasks/{taskId}", taskId);
        });

        app.MapPut("/tasks/{id}", async (Guid id, UpdateTaskCommand command, ICommandDispatcher dispatcher) =>
        {
            if (id != command.TaskId) return Results.BadRequest();

            var success = await dispatcher.Dispatch(command);
            if (!success) return Results.NotFound();

            return Results.NoContent();
        });

        app.MapDelete("/tasks/{id}", async (Guid id, ICommandDispatcher dispatcher) =>
        {
            var success = await dispatcher.Dispatch(new DeleteTaskCommand(id));
            if (!success) return Results.NotFound();

            return Results.NoContent();
        });
    }
}
