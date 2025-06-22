using Moq;
using TaskFlow.Application.Commands.UpdateTaskCommand;
using TaskFlow.Application.Interfaces;
using TaskFlow.Domain.TaskAggregate;
using TaskFlow.Domain.TaskAggregate.ValueObjects;

namespace TaskFlow.Application.Tests.CommandTests;

[TestClass]
public class UpdateTaskCommandHandlerTests
{
    private Mock<ITaskItemRepository> _mockRepository = null!;
    private UpdateTaskCommandHandler _handler = null!;

    [TestInitialize]
    public void Setup()
    {
        _mockRepository = new Mock<ITaskItemRepository>();
        _handler = new UpdateTaskCommandHandler(_mockRepository.Object);
    }



    [TestMethod]
    public async Task Handle_TaskExists_ShouldUpdateAndReturnTrue()
    {
        // Arrange
        var existingTask = TaskItem.Create("Old Title", "Old Desc", DateTime.Now.AddDays(1), Guid.NewGuid());
        var command = new UpdateTaskCommand(existingTask.Id, "New Title", "New Desc", DateTime.Now.AddDays(2));

        _mockRepository.Setup(r => r.GetByIdAsync(existingTask.Id, It.IsAny<CancellationToken>()))
            .ReturnsAsync(existingTask);

        _mockRepository.Setup(r => r.UpdateAsync(existingTask, It.IsAny<CancellationToken>()))
            .Returns(Task.CompletedTask)
            .Verifiable();

        // Act
        var result = await _handler.Handle(command);

        // Assert
        Assert.IsTrue(result);
        _mockRepository.Verify(r => r.UpdateAsync(It.Is<TaskItem>(t => t.Id == existingTask.Id && t.Title.Value == "New Title"), It.IsAny<CancellationToken>()), Times.Once);
    }
    
    [TestMethod]
    public async Task Handle_TaskDoesNotExist_ShouldReturnFalse()
    {
        // Arrange
        var command = new UpdateTaskCommand(Guid.NewGuid(), "Title", "Desc", DateTime.Now.AddDays(2));

        _mockRepository.Setup(r => r.GetByIdAsync(command.TaskId, It.IsAny<CancellationToken>()))
            .ReturnsAsync((TaskItem?)null);

        // Act
        var result = await _handler.Handle(command);

        // Assert
        Assert.IsFalse(result);
        _mockRepository.Verify(r => r.UpdateAsync(It.IsAny<TaskItem>(), It.IsAny<CancellationToken>()), Times.Never);
    }
}
