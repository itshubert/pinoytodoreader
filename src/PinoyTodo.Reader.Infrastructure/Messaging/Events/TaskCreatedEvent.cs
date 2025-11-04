namespace PinoyTodo.Reader.Infrastructure.Messaging.Events;

public sealed record TaskCreatedEvent(
    TaskId AggregateId,
    string Title,
    DateTimeOffset Timestamp);

public record TaskId(Guid Value);


