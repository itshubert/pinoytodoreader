namespace PinoyTodo.Reader.Infrastructure.Messaging;

public sealed class QueueSettings
{
    public string TaskCreated { get; set; } = string.Empty;
    public string TaskCompleted { get; set; } = string.Empty;
    public string TaskTitleUpdated { get; set; } = string.Empty;
    public string TaskDeleted { get; set; } = string.Empty;
}