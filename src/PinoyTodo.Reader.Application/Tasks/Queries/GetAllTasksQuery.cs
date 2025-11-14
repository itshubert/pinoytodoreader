using MapsterMapper;
using MediatR;
using PinoyTodo.Reader.Application.Common.Interfaces;
using PinoyTodo.Reader.Application.Common.Models;

namespace PinoyTodo.Reader.Application.Tasks.Queries;

public sealed record GetAllTasksQuery : IRequest<IEnumerable<TaskModel>>;

public sealed class GetAllTasksQueryHandler(ITaskRepository taskRepository, IMapper mapper) : IRequestHandler<GetAllTasksQuery, IEnumerable<TaskModel>>
{
    public async Task<IEnumerable<TaskModel>> Handle(
        GetAllTasksQuery request,
        CancellationToken cancellationToken)
    {
        return mapper.Map<IEnumerable<TaskModel>>(await taskRepository.GetAllAsync(cancellationToken));
    }
}