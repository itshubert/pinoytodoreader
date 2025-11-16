namespace PinoyTodo.Reader.Infrastructure.Messaging.Events;

public sealed record TaskTitleUpdatedEvent(
    TaskId AggregateId,
    string OldTitle,
    string NewTitle,
    DateTimeOffset Timestamp
);