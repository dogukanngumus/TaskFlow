using FluentValidation;
using FluentValidation.Results;
using Moq;
using TaskFlow.Application.Behaviours;
using TaskFlow.Application.Commands.CreateTaskCommand;

namespace TaskFlow.Application.Tests.Behaviours;

[TestClass]
public class ValidationBehaviourTests
{
    private Mock<IValidator<CreateTaskCommand>> _validatorMock = null!;
    private ValidationBehavior<CreateTaskCommand, Guid> _sut = null!;

    [TestInitialize]
    public void Setup()
    {
        _validatorMock = new Mock<IValidator<CreateTaskCommand>>();
        _sut = new ValidationBehavior<CreateTaskCommand, Guid>(new List<IValidator<CreateTaskCommand>> { _validatorMock.Object });
    }

    [TestMethod]
    public async Task Handle_WhenNoValidationErrors_CallsNext()
    {
        // Arrange
        var command = new CreateTaskCommand("Valid Title", "Valid Desc", DateTime.UtcNow.AddDays(1), Guid.NewGuid());
        _validatorMock.Setup(v => v.ValidateAsync(It.IsAny<ValidationContext<CreateTaskCommand>>(), It.IsAny<CancellationToken>()))
                      .ReturnsAsync(new ValidationResult());

        var called = false;
        Task<Guid> Next() { called = true; return Task.FromResult(Guid.NewGuid()); }

        // Act
        var result = await _sut.Handle(command, CancellationToken.None, Next);

        // Assert
        Assert.IsTrue(called, "Next delegate was not called");
        Assert.AreNotEqual(Guid.Empty, result);
    }


    [TestMethod]
    public async Task Handle_WhenValidationErrors_ThrowsValidationException()
    {
        // Arrange
        var command = new CreateTaskCommand("", "", DateTime.UtcNow.AddDays(-1), Guid.Empty);

        var failures = new List<ValidationFailure>
        {
            new ValidationFailure("Title", "Title is required."),
            new ValidationFailure("DueDate", "Due date must be in the future.")
        };

        _validatorMock.Setup(v => v.ValidateAsync(It.IsAny<ValidationContext<CreateTaskCommand>>(), It.IsAny<CancellationToken>()))
                      .ReturnsAsync(new ValidationResult(failures));

        Task<Guid> Next() => Task.FromResult(Guid.NewGuid());

        // Act & Assert
        await Assert.ThrowsExceptionAsync<ValidationException>(() => _sut.Handle(command, CancellationToken.None, Next));
    }


    [TestMethod]
    public async Task Handle_WhenNoValidators_CallsNext()
    {
        // Arrange
        var behaviorWithoutValidators = new ValidationBehavior<CreateTaskCommand, Guid>(new List<IValidator<CreateTaskCommand>>());
        var command = new CreateTaskCommand("Some Title", "Desc", DateTime.UtcNow.AddDays(1), Guid.NewGuid());

        var called = false;
        Task<Guid> Next() { called = true; return Task.FromResult(Guid.NewGuid()); }

        // Act
        var result = await behaviorWithoutValidators.Handle(command, CancellationToken.None, Next);

        // Assert
        Assert.IsTrue(called, "Next delegate was not called when no validators");
        Assert.AreNotEqual(Guid.Empty, result);
    }
}
