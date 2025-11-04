namespace PinoyTodo.Reader.Application.Common.Interfaces;

public interface ITaskRepository
{
    Task<IEnumerable<Domain.TaskAggregate.Task>> GetAllAsync(CancellationToken cancellationToken);
    Task<Domain.TaskAggregate.Task?> GetByIdAsync(Guid id, CancellationToken cancellationToken);
    Task UpdateAsync(Domain.TaskAggregate.Task task, CancellationToken cancellationToken);
}