using Microsoft.EntityFrameworkCore;
using PinoyTodo.Reader.Application.Common.Interfaces;

namespace PinoyTodo.Reader.Infrastructure.Persistence.Repositories;

public sealed class TaskRepository : BaseRepository, ITaskRepository
{
    public TaskRepository(TaskDbContext context) : base(context)
    {
    }

    public async Task<Domain.TaskAggregate.Task?> GetByIdAsync(Guid id, CancellationToken cancellationToken)
    {
        return await Context.Tasks.FindAsync(id, cancellationToken);
    }

    public async Task<IEnumerable<Domain.TaskAggregate.Task>> GetAllAsync(CancellationToken cancellationToken)
    {
        return await Context.Tasks.ToListAsync(cancellationToken);
    }

    public async Task UpdateAsync(Domain.TaskAggregate.Task task, CancellationToken cancellationToken)
    {
        Context.Tasks.Update(task);
        await SaveChangesAsync(cancellationToken);
    }
}