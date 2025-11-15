using MediatR;
using Microsoft.Extensions.Logging;
using PinoyTodo.Reader.Application.Common.Interfaces;
using PinoyTodo.Reader.Application.Tasks.Commands;
using PinoyTodo.Reader.Infrastructure.Messaging.Events;

namespace PinoyTodo.Reader.Infrastructure.Messaging.EventProcessors;

public sealed class TaskDeletedEventProcessor : IEventProcessor<TaskDeletedEvent>
{
    private readonly IMediator _mediator;
    private readonly ILogger<TaskDeletedEventProcessor> _logger;

    public TaskDeletedEventProcessor(
        ILogger<TaskDeletedEventProcessor> logger,
        IMediator mediator)
    {
        _logger = logger;
        _mediator = mediator;
    }

    public async Task<bool> ProcessEventAsync(TaskDeletedEvent @event, CancellationToken cancellationToken)
    {
        _logger.LogInformation("Processing TaskDeletedEvent for TaskId: {TaskId}", @event.AggregateId.Value);

        var command = new DeleteTaskCommand(@event.AggregateId.Value);

        var result = await _mediator.Send(command, cancellationToken);

        if (result.IsError)
        {
            foreach (var error in result.Errors)
            {
                _logger.LogError("Error deleting task {TaskId}: {ErrorCode} - {ErrorDescription}",
                    @event.AggregateId.Value, error.Code, error.Description);
            }
            return false;
        }

        return true;
    }
}