using ErrorOr;
using GeminiOrderFulfillment.Domain.Common.Errors;
using MapsterMapper;
using MediatR;
using Microsoft.Extensions.Logging;
using PinoyTodo.Reader.Application.Common.Interfaces;
using PinoyTodo.Reader.Application.Common.Models;

namespace PinoyTodo.Reader.Application.Tasks.Commands;

public sealed record CompleteTaskCommand(Guid Id) : IRequest<ErrorOr<TaskModel>>;

public sealed class CompleteTaskCommandHandler : IRequestHandler<CompleteTaskCommand, ErrorOr<TaskModel>>
{
    private readonly ITaskRepository _taskRepository;
    private readonly IMapper _mapper;
    private readonly ILogger<CreateTaskCommandHandler> _logger;

    public CompleteTaskCommandHandler(
        ITaskRepository taskRepository,
        IMapper mapper,
        ILogger<CreateTaskCommandHandler> logger)
    {
        _taskRepository = taskRepository;
        _mapper = mapper;
        _logger = logger;
    }

    public async Task<ErrorOr<TaskModel>> Handle(CompleteTaskCommand request, CancellationToken cancellationToken)
    {
        var task = await _taskRepository.GetByIdAsync(request.Id, cancellationToken);
        if (task is null)
        {
            _logger.LogWarning("Task not found: {TaskId}", request.Id);
            return Errors.Task.NotFound;
        }

        task.MarkAsCompleted();
        await _taskRepository.UpdateAsync(task, cancellationToken);

        _logger.LogInformation("Task completed: {TaskId}", request.Id);
        return _mapper.Map<TaskModel>(task);
    }
}