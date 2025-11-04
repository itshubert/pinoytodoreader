namespace PinoyTodo.Reader.Infrastructure.Persistence.Repositories;

public abstract class BaseRepository
{
    protected readonly TaskDbContext Context;

    protected BaseRepository(TaskDbContext context)
    {
        Context = context;
    }

    public async Task SaveChangesAsync(CancellationToken cancellationToken)
    {
        await Context.SaveChangesAsync(cancellationToken);
    }

    public async Task BeginTransactionAsync(CancellationToken cancellationToken)
    {
        await Context.Database.BeginTransactionAsync(cancellationToken);
    }

    public async Task CommitTransactionAsync(CancellationToken cancellationToken)
    {
        await Context.Database.CommitTransactionAsync(cancellationToken);
    }

    public async Task RollbackTransactionAsync(CancellationToken cancellationToken)
    {
        await Context.Database.RollbackTransactionAsync(cancellationToken);
    }
}