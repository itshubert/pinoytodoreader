using MapsterMapper;
using MediatR;
using Microsoft.Extensions.Logging;
using PinoyTodo.Reader.Application.Common.Interfaces;
using PinoyTodo.Reader.Application.Common.Models;

namespace PinoyTodo.Reader.Application.Tasks.Commands;

public sealed record CreateTaskCommand(Guid Id, string Title) : IRequest<TaskModel>;

public sealed class CreateTaskCommandHandler : IRequestHandler<CreateTaskCommand, TaskModel>
{
    private readonly ITaskRepository _taskRepository;
    private readonly IMapper _mapper;
    private readonly ILogger<CreateTaskCommandHandler> _logger;

    public CreateTaskCommandHandler(
        ITaskRepository taskRepository,
        IMapper mapper,
        ILogger<CreateTaskCommandHandler> logger)
    {
        _taskRepository = taskRepository;
        _mapper = mapper;
        _logger = logger;
    }

    public async Task<TaskModel> Handle(CreateTaskCommand request, CancellationToken cancellationToken)
    {
        var task = Domain.TaskAggregate.Task.Create(
            request.Id,
            request.Title,
            isCompleted: false);

        await _taskRepository.AddAsync(task, cancellationToken);

        _logger.LogInformation("Task with ID {TaskId} created.", task.Id);

        return _mapper.Map<TaskModel>(task);
    }
}