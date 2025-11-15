using ErrorOr;
using MediatR;
using PinoyTodo.Reader.Application.Common.Interfaces;
using PinoyTodo.Reader.Application.Common.Models;

namespace PinoyTodo.Reader.Application.Tasks.Commands;

public sealed record DeleteTaskCommand(Guid Id) : IRequest<ErrorOr<Unit>>;

public sealed class DeleteTaskCommandHandler : IRequestHandler<DeleteTaskCommand, ErrorOr<Unit>>
{
    private readonly ITaskRepository _taskRepository;

    public DeleteTaskCommandHandler(ITaskRepository taskRepository)
    {
        _taskRepository = taskRepository;
    }

    public async Task<ErrorOr<Unit>> Handle(DeleteTaskCommand request, CancellationToken cancellationToken)
    {
        var task = await _taskRepository.GetByIdAsync(request.Id, cancellationToken);
        if (task is null)
        {
            return Domain.Common.Errors.Errors.Task.NotFound;
        }

        await _taskRepository.DeleteAsync(task, cancellationToken);

        return Unit.Value;
    }
}