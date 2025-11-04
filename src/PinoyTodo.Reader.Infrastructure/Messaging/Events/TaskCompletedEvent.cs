namespace PinoyTodo.Reader.Infrastructure.Messaging.Events;

public sealed record TaskCompletedEvent(
    TaskId AggregateId,
    DateTimeOffset CompletionTime,
    DateTimeOffset Timestamp
);