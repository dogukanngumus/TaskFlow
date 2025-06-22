using System;
using FluentValidation;

namespace TaskFlow.Application.Commands.Validators;

public class DeleteTaskCommandValidator : AbstractValidator<TaskFlow.Application.Commands.DeleteTaskCommand.DeleteTaskCommand>
{
    public DeleteTaskCommandValidator()
    {
        RuleFor(x => x.TaskId)
            .NotEmpty().WithMessage("TaskId is required.");
    }
}