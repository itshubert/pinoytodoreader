using ErrorOr;

namespace PinoyTodo.Reader.Domain.Common.Errors;

public static partial class Errors
{
    public static class Task
    {
        public static Error NotFound => Error.NotFound(
            code: "Task.NotFound",
            description: "The specified task was not found.");

        public static Error InvalidTaskId(Guid taskId) => Error.Validation(
            code: "Task.InvalidId",
            description: $"The task with ID '{taskId}' is invalid or does not exist.");

        public static Error TaskAlreadyExists(Guid taskId) => Error.Conflict(
            code: "Task.AlreadyExists",
            description: $"A task with ID '{taskId}' already exists.");

        public static Error TaskAlreadyCompleted => Error.Validation(
            code: "Task.AlreadyCompleted",
            description: $"The task is already completed.");
    }
}