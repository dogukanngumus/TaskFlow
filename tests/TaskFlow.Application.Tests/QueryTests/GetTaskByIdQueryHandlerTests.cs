using System;
using Moq;
using TaskFlow.Application.Interfaces;
using TaskFlow.Application.Queries.GetTasksById;
using TaskFlow.Domain.TaskAggregate;

namespace TaskFlow.Application.Tests.QueryTests;

[TestClass]
public class GetTaskByIdQueryHandlerTests
{
    private Mock<ITaskItemRepository> _mockRepository = null!;
    private GetTaskByIdQueryHandler _handler = null!;

    [TestInitialize]
    public void Setup()
    {
        _mockRepository = new Mock<ITaskItemRepository>();
        _handler = new GetTaskByIdQueryHandler(_mockRepository.Object);
    }

    [TestMethod]
    public async Task Handle_TaskExists_ReturnsTaskDto()
    {
        // Arrange
        var task = TaskItem.Create("Example Task", "Some Description", DateTime.Today,Guid.NewGuid());
        var query = new GetTaskByIdQuery(task.Id);

        _mockRepository.Setup(r => r.GetByIdAsync(task.Id, It.IsAny<CancellationToken>()))
            .ReturnsAsync(task);

        // Act
        var result = await _handler.Handle(query);

        // Assert
        Assert.IsNotNull(result);
        Assert.AreEqual(task.Id, result.Id);
        Assert.AreEqual(task.Title.Value, result.Title);
        Assert.AreEqual(task.Description, result.Description);
        Assert.AreEqual(task.DueDate, result.DueDate);
    }

    [TestMethod]
    public async Task Handle_TaskNotFound_ReturnsNull()
    {
        // Arrange
        var taskId = Guid.NewGuid();
        var query = new GetTaskByIdQuery(taskId);

        _mockRepository.Setup(r => r.GetByIdAsync(taskId, It.IsAny<CancellationToken>()))
            .ReturnsAsync((TaskItem?)null);

        // Act
        var result = await _handler.Handle(query);

        // Assert
        Assert.IsNull(result);
    }
}
