using Moq;
using TaskFlow.Application.Commands.DeleteTaskCommand;
using TaskFlow.Application.Interfaces;
using TaskFlow.Domain.TaskAggregate;

namespace TaskFlow.Application.Tests;

[TestClass]
public class DeleteTaskCommandHandlerTests
{
    private Mock<ITaskItemRepository> _mockRepository = null!;
    private DeleteTaskCommandHandler _handler = null!;

    [TestInitialize]
    public void Setup()
    {
        _mockRepository = new Mock<ITaskItemRepository>();
        _handler = new DeleteTaskCommandHandler(_mockRepository.Object);
    }

    [TestMethod]
    public async Task Handle_TaskExists_ShouldDeleteAndReturnTrue()
    {
        // Arrange
        var existingTask = TaskItem.Create("Old Title", "Old Desc", DateTime.Now.AddDays(1), Guid.NewGuid());

        _mockRepository.Setup(r => r.GetByIdAsync(existingTask.Id, It.IsAny<CancellationToken>()))
            .ReturnsAsync(existingTask);

        _mockRepository.Setup(r => r.DeleteAsync(existingTask, It.IsAny<CancellationToken>()))
            .Returns(Task.CompletedTask)
            .Verifiable();

        var command = new DeleteTaskCommand(existingTask.Id);

        // Act
        var result = await _handler.Handle(command);

        // Assert
        Assert.IsTrue(result);
        _mockRepository.Verify(r => r.DeleteAsync(existingTask, It.IsAny<CancellationToken>()), Times.Once);
    }
    
    [TestMethod]
    public async Task Handle_TaskDoesNotExist_ShouldReturnFalse()
    {
        // Arrange
        var command = new DeleteTaskCommand(Guid.NewGuid());

        _mockRepository.Setup(r => r.GetByIdAsync(command.TaskId, It.IsAny<CancellationToken>()))
            .ReturnsAsync((TaskItem?)null);

        // Act
        var result = await _handler.Handle(command);

        // Assert
        Assert.IsFalse(result);
        _mockRepository.Verify(r => r.DeleteAsync(It.IsAny<TaskItem>(), It.IsAny<CancellationToken>()), Times.Never);
    }
}
