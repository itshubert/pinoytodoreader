using PinoyCleanArch.Domain.Common.Models;

namespace PinoyTodo.Reader.Domain.TaskAggregate;

public sealed class Task : AggregateRoot<Guid>
{
    public string Title { get; private set; }
    public bool IsCompleted { get; private set; }
    public DateTimeOffset? CompletionTime { get; private set; }

    private Task(
        Guid id,
        string title,
        bool isCompleted,
        DateTimeOffset? completionTime = null)
        : base(id)
    {
        Title = title;
        IsCompleted = isCompleted;
        CompletionTime = completionTime;
    }
    public static Task Create(
        Guid? id,
        string title,
        bool isCompleted,
        DateTimeOffset? completionTime = null
    )
    {
        return new Task(
            id ?? Guid.NewGuid(),
            title,
            isCompleted,
            completionTime
        );
    }

    public void MarkAsCompleted()
    {
        IsCompleted = true;
    }
}