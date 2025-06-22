using TaskFlow.Domain.TaskAggregate;
using TaskFlow.Domain.TaskAggregate.Exceptions;

namespace TaskFlow.Domain.Tests;

[TestClass]
public class TaskItemTests
{

    [TestMethod]
    public void Create_ShouldInitializeWithToDoStatus()
    {
        // Arrange
        var userId = Guid.NewGuid();
        var dueDate = DateTime.UtcNow.AddDays(1);

        // Act
        var task = TaskItem.Create("Test task", "A description", dueDate, userId);

        // Assert
        Assert.AreEqual(TaskFlow.Domain.TaskAggregate.TaskStatus.ToDo, task.Status);
        Assert.AreEqual("Test task", task.Title.Value);
    }

    [TestMethod]
    [ExpectedException(typeof(InvalidDueDateException))]
    public void Create_ShouldThrow_WhenDueDateIsInPast()
    {
        // Act
        TaskItem.Create("Past Task", "Some description", DateTime.UtcNow.AddDays(-1), Guid.NewGuid());
    }

    [TestMethod]
    [ExpectedException(typeof(ArgumentException))]
    public void Create_ShouldThrow_WhenTitleIsEmpty()
    {
        // Act
        TaskItem.Create("", "Some description", DateTime.UtcNow.AddDays(1), Guid.NewGuid());
    }

    [TestMethod]
    public void MoveToInProgress_ShouldChangeStatus()
    {
        var task = TaskItem.Create("A task", "desc", DateTime.UtcNow.AddDays(1), Guid.NewGuid());

        task.MoveToInProgress();

        Assert.AreEqual(TaskFlow.Domain.TaskAggregate.TaskStatus.InProgress, task.Status);
    }

    [TestMethod]
    public void Complete_ShouldChangeStatusToDone()
    {
        var task = TaskItem.Create("A task", "desc", DateTime.UtcNow.AddDays(1), Guid.NewGuid());

        task.Complete();

        Assert.AreEqual(TaskFlow.Domain.TaskAggregate.TaskStatus.Done, task.Status);
    }

    [TestMethod]
    [ExpectedException(typeof(InvalidTaskStatusException))]
    public void MoveToInProgress_ShouldThrow_IfAlreadyDone()
    {
        var task = TaskItem.Create("task", "desc", DateTime.UtcNow.AddDays(1), Guid.NewGuid());
        task.Complete();

        task.MoveToInProgress();
    }

    [TestMethod]
    public void Update_ShouldModifyTitleAndDescriptionAndDueDate()
    {
        var task = TaskItem.Create("Old", "desc", DateTime.UtcNow.AddDays(1), Guid.NewGuid());
        var newDueDate = DateTime.UtcNow.AddDays(3);

        task.Update("New title", "Updated desc", newDueDate);

        Assert.AreEqual("New title", task.Title.Value);
        Assert.AreEqual("Updated desc", task.Description);
        Assert.AreEqual(newDueDate, task.DueDate);
    }
}
