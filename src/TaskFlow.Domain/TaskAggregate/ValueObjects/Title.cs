namespace TaskFlow.Domain.TaskAggregate.ValueObjects;

public class Title
{
    public string Value { get; }

    private Title()
    {
        
    }

    public Title(string value)
    {
        if (string.IsNullOrWhiteSpace(value))
            throw new ArgumentException("Title cannot be empty.");

        if (value.Length > 100)
            throw new ArgumentException("Title is too long.");

        Value = value;

    }
    
    public override bool Equals(object? obj) => obj is Title other && Value == other.Value;

    public override int GetHashCode() => Value.GetHashCode();

    public static implicit operator string(Title title) => title.Value;

    public static explicit operator Title(string value) => new Title(value);
}
