using MediatR;
using Microsoft.Extensions.Logging;
using PinoyTodo.Reader.Application.Common.Interfaces;
using PinoyTodo.Reader.Application.Tasks.Commands;
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

        var command = new CompleteTaskCommand(@event.AggregateId.Value);

        var result = await _mediator.Send(command, cancellationToken);

        if (result.IsError)
        {
            foreach (var error in result.Errors)
            {
                _logger.LogError("Error completing task {TaskId}: {ErrorCode} - {ErrorDescription}",
                    @event.AggregateId.Value, error.Code, error.Description);
            }
            return false;
        }

        return true;
    }
}