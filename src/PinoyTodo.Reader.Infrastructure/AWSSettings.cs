namespace PinoyTodo.Reader.Infrastructure;

public sealed class AWSSettings
{
    public string Region { get; set; } = string.Empty;
    public string ServiceUrl { get; set; } = string.Empty;
    public string QueueUrl { get; set; } = string.Empty;
    public bool UseLocalStack { get; set; } = false;
}
