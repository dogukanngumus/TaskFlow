using System;
using FluentValidation;

namespace TaskFlow.Application.Commands.Validators;

public class UpdateTaskCommandValidator: AbstractValidator<TaskFlow.Application.Commands.UpdateTaskCommand.UpdateTaskCommand>
{
    public UpdateTaskCommandValidator()
    {
        RuleFor(x => x.TaskId).NotEmpty().WithMessage("TaskId is required.");

        RuleFor(x => x.Title)
            .NotEmpty().WithMessage("Title cannot be empty.")
            .MaximumLength(100).WithMessage("Title cannot be longer than 100 characters.");

        RuleFor(x => x.DueDate)
            .Must(dueDate => dueDate > DateTime.UtcNow)
            .WithMessage("DueDate must be in the future.");
    }
}
