using Mapster;

namespace PinoyTodo.Reader.Application.Common.Mappings;

public sealed class TaskMappingConfig : IRegister
{
    public void Register(TypeAdapterConfig config)
    {
        config.NewConfig<Domain.TaskAggregate.Task, Models.TaskModel>()
            .Map(dest => dest, src => src);

    }
}

