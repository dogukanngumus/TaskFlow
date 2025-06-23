using Moq;
using TaskFlow.Application.Commands.CreateTaskCommand;
using TaskFlow.Application.Interfaces;
using TaskFlow.Domain.TaskAggregate;

namespace TaskFlow.Application.Tests.CommandTests;

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

    [TestMethod]
    public async Task Handle_WhenDomainThrowsException_ShouldPropagate()
    {
        // Arrange
        var command = new CreateTaskCommand("Valid Title", "Valid Desc", DateTime.UtcNow.AddDays(1), Guid.NewGuid());


        _mockRepository.Setup(r => r.AddAsync(It.IsAny<TaskItem>(), It.IsAny<CancellationToken>()))
                    .ThrowsAsync(new Exception("Simulated failure"));

        // Act & Assert
        await Assert.ThrowsExceptionAsync<Exception>(() => _handler.Handle(command));
    }

    [TestMethod]
    public async Task Handle_WhenRepositoryThrowsException_ShouldPropagateException()
    {
        // Arrange
        var command = new CreateTaskCommand("Valid Title", "Valid Desc", DateTime.UtcNow.AddDays(1), Guid.NewGuid());

        _mockRepository.Setup(r => r.AddAsync(It.IsAny<TaskItem>(), It.IsAny<CancellationToken>()))
                    .ThrowsAsync(new Exception("Simulated failure"));

        // Act & Assert
        await Assert.ThrowsExceptionAsync<Exception>(() => _handler.Handle(command));
    }
}
