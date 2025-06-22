using System;
using Moq;
using TaskFlow.Application.Commands.CreateTaskCommand;
using TaskFlow.Application.Interfaces;
using TaskFlow.Domain.TaskAggregate;

namespace TaskFlow.Application.Tests;

[TestClass]
public class CreateTaskCommandHandlerTests
{
    private Mock<ITaskItemRepository> _mockRepository;
    private CreateTaskCommandHandler _handler;

    [TestInitialize]
    public void Setup()
    {
        _mockRepository = new Mock<ITaskItemRepository>();
        _handler = new CreateTaskCommandHandler(_mockRepository.Object);
    }

    [TestMethod]
    public async Task Handle_ValidCommand_CreatesTaskAndCallsRepository()
    {
        // Arrange
        var command = new CreateTaskCommand("Test Title", "Test Description", DateTime.Now.AddDays(1), Guid.NewGuid());
        _mockRepository.Setup(r => r.AddAsync(It.IsAny<TaskItem>(), It.IsAny<CancellationToken>()))
                       .Returns(Task.CompletedTask)
                       .Verifiable();
        // Act
        var result = await _handler.Handle(command);

        // Assert
        _mockRepository.Verify(r => r.AddAsync(It.Is<TaskItem>(t => t.Title.Value == "Test Title"), It.IsAny<CancellationToken>()), Times.Once);
        Assert.AreNotEqual(Guid.Empty, result);
    }
}
