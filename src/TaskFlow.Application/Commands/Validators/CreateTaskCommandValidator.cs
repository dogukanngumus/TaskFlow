using FluentValidation;

namespace TaskFlow.Application.Commands.Validators;

 public class CreateTaskCommandValidator : AbstractValidator<TaskFlow.Application.Commands.CreateTaskCommand.CreateTaskCommand>
{
    public CreateTaskCommandValidator()
    {
        RuleFor(x => x.Title)
            .NotEmpty().WithMessage("Title is required.")
            .MaximumLength(100).WithMessage("Title cannot exceed 100 characters.");

        RuleFor(x => x.Description).MaximumLength(500).WithMessage("Description cannot exceed 500 characters.");

        RuleFor(x => x.DueDate).Must(BeAValidDate).WithMessage("Due date must be in the future.");

        RuleFor(x => x.UserId).NotEmpty().WithMessage("UserId is required.");
    }

    private bool BeAValidDate(DateTime date)
    {
        return date > DateTime.UtcNow;
    }
}
