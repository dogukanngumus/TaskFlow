using Microsoft.EntityFrameworkCore;
using TaskFlow.Infrastructure.Persistence;
using TaskFlow.Infrastructure.Repositories;
using TaskFlow.Domain.TaskAggregate;
using TaskFlow.Application.Interfaces;

namespace TaskFlow.Infrastructure.Tests;

[TestClass]
public class TaskItemRepositoryTests
{
    private TaskDbContext _dbContext = null!;
    private ITaskItemRepository _repository = null!;

    [TestInitialize]
    public void Setup()
    {
        var options = new DbContextOptionsBuilder<TaskDbContext>()
            .UseInMemoryDatabase(databaseName: Guid.NewGuid().ToString()) // izole her test için
            .Options;

        _dbContext = new TaskDbContext(options);
        _repository = new TaskItemRepository(_dbContext);
    }

    [TestMethod]
    public async Task AddAsync_ShouldAddTask()
    {
        var task = TaskItem.Create("Test", "desc", DateTime.UtcNow.AddDays(1), Guid.NewGuid());

        await _repository.AddAsync(task);
        await _repository.SaveChangesAsync();

        var result = await _dbContext.TaskItems.FindAsync(task.Id);

        Assert.IsNotNull(result);
        Assert.AreEqual(task.Title.Value, result!.Title.Value);
    }

    [TestMethod]
    public async Task GetByIdAsync_ShouldReturnCorrectTask()
    {
        var task = TaskItem.Create("Sample", "desc", DateTime.UtcNow.AddDays(1), Guid.NewGuid());
        _dbContext.TaskItems.Add(task);
        await _dbContext.SaveChangesAsync();

        var result = await _repository.GetByIdAsync(task.Id);

        Assert.IsNotNull(result);
        Assert.AreEqual(task.Id, result!.Id);
    }

    [TestMethod]
    public async Task GetAllAsync_ShouldReturnAllTasks()
    {
        _dbContext.TaskItems.Add(TaskItem.Create("1", "desc", DateTime.UtcNow.AddDays(1), Guid.NewGuid()));
        _dbContext.TaskItems.Add(TaskItem.Create("2", "desc", DateTime.UtcNow.AddDays(1), Guid.NewGuid()));
        await _dbContext.SaveChangesAsync();

        var result = await _repository.GetAllAsync();

        Assert.AreEqual(2, result.Count());
    }

    [TestMethod]
    public async Task UpdateAsync_ShouldModifyTask()
    {
        var task = TaskItem.Create("Original", "desc", DateTime.UtcNow.AddDays(1), Guid.NewGuid());
        _dbContext.TaskItems.Add(task);
        await _dbContext.SaveChangesAsync();

        task.Update("Updated", "new desc", DateTime.UtcNow.AddDays(2));
        await _repository.UpdateAsync(task);
        await _repository.SaveChangesAsync();

        var updated = await _dbContext.TaskItems.FindAsync(task.Id);

        Assert.IsNotNull(updated);
        Assert.AreEqual("Updated", updated!.Title.Value);
    }

    [TestMethod]
    public async Task DeleteAsync_ShouldRemoveTask()
    {
        var task = TaskItem.Create("ToDelete", "desc", DateTime.UtcNow.AddDays(1), Guid.NewGuid());
        _dbContext.TaskItems.Add(task);
        await _dbContext.SaveChangesAsync();

        await _repository.DeleteAsync(task);
        await _repository.SaveChangesAsync();

        var deleted = await _dbContext.TaskItems.FindAsync(task.Id);
        Assert.IsNull(deleted);
    }
}
