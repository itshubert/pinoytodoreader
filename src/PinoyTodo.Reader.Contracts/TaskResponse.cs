namespace PinoyTodo.Reader.Contracts;

public sealed record TaskResponse(
    Guid Id,
    string Title,
    bool IsCompleted,
    DateTimeOffset? CompletionTime);