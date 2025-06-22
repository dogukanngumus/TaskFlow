namespace TaskFlow.Domain.TaskAggregate.Exceptions;

public class InvalidTaskStatusException : Exception
{
    public InvalidTaskStatusException(string message) : base(message) { }
}