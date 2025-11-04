namespace PinoyTodo.Reader.Application.Common.Interfaces;

public interface IEventProcessor<TEvent>
{
    Task<bool> ProcessEventAsync(TEvent @event, CancellationToken cancellationToken);
}