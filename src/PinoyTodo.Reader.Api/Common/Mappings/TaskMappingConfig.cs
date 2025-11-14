using Mapster;

namespace PinoyTodo.Reader.Api.Common.Mappings;

public sealed class TaskMappingConfig : IRegister
{
    public void Register(TypeAdapterConfig config)
    {
        config.NewConfig<Application.Common.Models.TaskModel, Contracts.TaskResponse>();
    }
}