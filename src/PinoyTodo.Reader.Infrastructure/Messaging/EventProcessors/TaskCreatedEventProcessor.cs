using MediatR;
using Microsoft.Extensions.Logging;
using PinoyTodo.Reader.Application.Common.Interfaces;
using PinoyTodo.Reader.Application.Tasks.Commands;
using PinoyTodo.Reader.Infrastructure.Messaging.Events;

namespace PinoyTodo.Reader.Infrastructure.Messaging.EventProcessors;

public sealed class TaskCreatedEventProcessor : IEventProcessor<TaskCreatedEvent>
{
    private readonly IMediator _mediator;
    private readonly ILogger<TaskCreatedEventProcessor> _logger;

    public TaskCreatedEventProcessor(
        ILogger<TaskCreatedEventProcessor> logger,
        IMediator mediator)
    {
        _logger = logger;
        _mediator = mediator;
    }

    public async Task<bool> ProcessEventAsync(TaskCreatedEvent @event, CancellationToken cancellationToken)
    {
        _logger.LogInformation("Processing TaskCreatedEvent for TaskId: {TaskId}", @event.AggregateId.Value);

        await _mediator.Send(
            new CreateTaskCommand(
                @event.AggregateId.Value,
                @event.Title),
            cancellationToken);

        _logger.LogInformation("TaskCreatedEvent processed successfully for TaskId: {TaskId}", @event.AggregateId.Value);

        return true;
    }
}