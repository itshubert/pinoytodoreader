using ErrorOr;
using MapsterMapper;
using MediatR;
using PinoyTodo.Reader.Application.Common.Interfaces;
using PinoyTodo.Reader.Application.Common.Models;
using PinoyTodo.Reader.Domain.Common.Errors;

namespace PinoyTodo.Reader.Application.Tasks.Commands;

public sealed record UpdateTaskTitleCommand(Guid Id, string NewTitle) : IRequest<ErrorOr<TaskModel>>;

public sealed class UpdateTaskTitleCommandHandler : IRequestHandler<UpdateTaskTitleCommand, ErrorOr<TaskModel>>
{
    private readonly ITaskRepository _taskRepository;
    private readonly IMapper _mapper;

    public UpdateTaskTitleCommandHandler(
        ITaskRepository taskRepository,
        IMapper mapper)
    {
        _taskRepository = taskRepository;
        _mapper = mapper;
    }

    public async Task<ErrorOr<TaskModel>> Handle(UpdateTaskTitleCommand request, CancellationToken cancellationToken)
    {
        var task = await _taskRepository.GetByIdAsync(request.Id, cancellationToken);
        if (task is null)
        {
            return Errors.Task.NotFound;
        }

        task.UpdateTitle(request.NewTitle);
        await _taskRepository.UpdateAsync(task, cancellationToken);

        return _mapper.Map<TaskModel>(task);
    }
}