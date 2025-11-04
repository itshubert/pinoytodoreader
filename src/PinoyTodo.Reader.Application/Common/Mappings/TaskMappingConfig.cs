using Mapster;

namespace PinoyTodo.Reader.Application.Common.Mappings;

public sealed class TaskMappingConfig : IRegister
{
    public void Register(TypeAdapterConfig config)
    {
        config.NewConfig<Domain.TaskAggregate.Task, Models.TaskModel>()
            .Map(dest => dest.Id, src => src.Id)
            .Map(dest => dest, src => src);

    }
}

