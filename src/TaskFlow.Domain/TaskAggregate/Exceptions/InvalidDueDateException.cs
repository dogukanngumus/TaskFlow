namespace TaskFlow.Domain.TaskAggregate.Exceptions;

public class InvalidDueDateException : Exception
{
    public InvalidDueDateException(string message) : base(message) { }
}
