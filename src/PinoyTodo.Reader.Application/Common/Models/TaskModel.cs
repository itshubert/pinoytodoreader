namespace PinoyTodo.Reader.Application.Common.Models;

public sealed record TaskModel(
    Guid Id,
    string Title,
    bool IsCompleted,
    DateTimeOffset? CompletionTime
);