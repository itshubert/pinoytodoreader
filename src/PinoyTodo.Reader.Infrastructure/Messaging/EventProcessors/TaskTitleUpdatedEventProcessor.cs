using MediatR;
using Microsoft.Extensions.Logging;
using PinoyTodo.Reader.Application.Common.Interfaces;
using PinoyTodo.Reader.Application.Tasks.Commands;
using PinoyTodo.Reader.Infrastructure.Messaging.Events;

namespace PinoyTodo.Reader.Infrastructure.Messaging.EventProcessors;

public sealed class TaskTitleUpdatedEventProcessor : IEventProcessor<TaskTitleUpdatedEvent>
{
    private readonly IMediator _mediator;
    private readonly ILogger<TaskTitleUpdatedEventProcessor> _logger;

    public TaskTitleUpdatedEventProcessor(
        ILogger<TaskTitleUpdatedEventProcessor> logger,
        IMediator mediator)
    {
        _logger = logger;
        _mediator = mediator;
    }

    public async Task<bool> ProcessEventAsync(TaskTitleUpdatedEvent @event, CancellationToken cancellationToken)
    {
        _logger.LogInformation("Processing TaskTitleUpdatedEvent for TaskId: {TaskId}", @event.AggregateId.Value);

        var command = new UpdateTaskTitleCommand(@event.AggregateId.Value, @event.NewTitle);

        var result = await _mediator.Send(command, cancellationToken);

        if (result.IsError)
        {
            foreach (var error in result.Errors)
            {
                _logger.LogError("Error updating title for task {TaskId}: {ErrorCode} - {ErrorDescription}",
                    @event.AggregateId.Value, error.Code, error.Description);
            }
            return false;
        }

        return true;
    }
}
