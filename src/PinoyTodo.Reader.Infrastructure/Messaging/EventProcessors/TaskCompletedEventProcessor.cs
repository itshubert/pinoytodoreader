using MediatR;
using Microsoft.Extensions.Logging;
using PinoyTodo.Reader.Application.Common.Interfaces;
using PinoyTodo.Reader.Infrastructure.Messaging.Events;

namespace PinoyTodo.Reader.Infrastructure.Messaging.EventProcessors;

public sealed class TaskCompletedEventProcessor : IEventProcessor<TaskCompletedEvent>
{
    private readonly IMediator _mediator;
    private readonly ILogger<TaskCompletedEventProcessor> _logger;

    public TaskCompletedEventProcessor(
        ILogger<TaskCompletedEventProcessor> logger,
        IMediator mediator)
    {
        _logger = logger;
        _mediator = mediator;
    }

    public async Task<bool> ProcessEventAsync(TaskCompletedEvent @event, CancellationToken cancellationToken)
    {
        _logger.LogInformation("Processing TaskCompletedEvent for TaskId: {TaskId}", @event.AggregateId.Value);

        return await ValueTask.FromResult(true);
    }
}