namespace PinoyTodo.Reader.Infrastructure.Messaging.Events;

public sealed record TaskDeletedEvent(
    TaskId AggregateId,
    DateTimeOffset Timestamp
);