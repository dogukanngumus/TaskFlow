using System;
using Microsoft.Extensions.Logging;
using Moq;
using TaskFlow.Application.Behaviours;
using TaskFlow.Application.Commands.CreateTaskCommand;

namespace TaskFlow.Application.Tests.Behaviours;

[TestClass]
public class LoggingBehaviorTests
{
    private LoggingBehavior<CreateTaskCommand, Guid> _behavior;
    private Mock<ILogger<LoggingBehavior<CreateTaskCommand, Guid>>> _loggerMock;
    private CreateTaskCommand _command;

    [TestInitialize]
    public void Setup()
    {
        _loggerMock = new Mock<ILogger<LoggingBehavior<CreateTaskCommand, Guid>>>();
        _behavior = new LoggingBehavior<CreateTaskCommand, Guid>(_loggerMock.Object);
        _command = new CreateTaskCommand("Title", "Desc", DateTime.UtcNow.AddDays(1), Guid.NewGuid());
    }

    [TestMethod]
    public async Task Handle_ShouldLogInformationAndCallNext()
    {
        // Arrange
        var nextCalled = false;
        RequestHandlerDelegate<Guid> next = () =>
        {
            nextCalled = true;
            return Task.FromResult(Guid.NewGuid());
        };

        // Act
        var result = await _behavior.Handle(_command, default, next);

        // Assert
        Assert.IsTrue(nextCalled);
        _loggerMock.Verify(
            x => x.Log(
                LogLevel.Information,
                It.IsAny<EventId>(),
                It.Is<It.IsAnyType>((v, t) => v.ToString()!.Contains("Handling")),
                null,
                It.IsAny<Func<It.IsAnyType, Exception?, string>>()), Times.Once);

        _loggerMock.Verify(
            x => x.Log(
                LogLevel.Information,
                It.IsAny<EventId>(),
                It.Is<It.IsAnyType>((v, t) => v.ToString()!.Contains("Handled")),
                null,
                It.IsAny<Func<It.IsAnyType, Exception?, string>>()), Times.Once);
    }

    [TestMethod]
    public async Task Handle_WhenExceptionOccurs_ShouldLogError()
    {
        // Arrange
        RequestHandlerDelegate<Guid> next = () =>
        {
            throw new Exception("Test exception");
        };

        // Act & Assert
        await Assert.ThrowsExceptionAsync<Exception>(() => _behavior.Handle(_command, default, next));

        _loggerMock.Verify(
            x => x.Log(
                LogLevel.Error,
                It.IsAny<EventId>(),
                It.Is<It.IsAnyType>((v, t) => v.ToString()!.Contains("Error handling")),
                It.IsAny<Exception>(),
                It.IsAny<Func<It.IsAnyType, Exception?, string>>()), Times.Once);
    }
}

