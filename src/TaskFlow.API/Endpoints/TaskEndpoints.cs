using System.Threading.RateLimiting;
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
        })
        .RequireRateLimiting()
        .WithName("GetAllTasks")
        .WithOpenApi(operation =>
        {
            operation.Summary = "Get all tasks";
            operation.Description = "Returns a list of all tasks.";
            return operation;
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
        })
        .RequireRateLimiting()
        .WithName("GetTaskById")
        .WithOpenApi(operation =>
        {
            operation.Summary = "Get task by ID";
            operation.Description = "Returns a single task by the specified ID.";
            return operation;
        });

        app.MapPost("/tasks", async (CreateTaskCommand command, ICommandDispatcher dispatcher) =>
        {
            var taskId = await dispatcher.Dispatch(command);
            return Results.Created($"/tasks/{taskId}", taskId);
        })
        .RequireRateLimiting()
        .WithName("CreateTask")
        .WithOpenApi(operation =>
        {
            operation.Summary = "Create a new task";
            operation.Description = "Creates a new task with the provided details.";
            return operation;
        });

        app.MapPut("/tasks/{id}", async (Guid id, UpdateTaskCommand command, ICommandDispatcher dispatcher) =>
        {
            if (id != command.TaskId) return Results.BadRequest();

            var success = await dispatcher.Dispatch(command);
            if (!success) return Results.NotFound();

            return Results.NoContent();
        })
        .RequireRateLimiting()
        .WithName("UpdateTask")
        .WithOpenApi(operation =>
        {
            operation.Summary = "Update an existing task";
            operation.Description = "Updates the task with the specified ID.";
            return operation;
        });

        app.MapDelete("/tasks/{id}", async (Guid id, ICommandDispatcher dispatcher) =>
        {
            var success = await dispatcher.Dispatch(new DeleteTaskCommand(id));
            if (!success) return Results.NotFound();

            return Results.NoContent();
        })
        .RequireRateLimiting()
        .WithName("DeleteTask")
        .WithOpenApi(operation =>
        {
            operation.Summary = "Delete a task";
            operation.Description = "Deletes the task with the specified ID.";
            return operation;
        });
    }

    private static RouteHandlerBuilder RequireRateLimiting(this RouteHandlerBuilder builder)
    {
        return builder.RequireRateLimiting("global");
    }
}
