using Microsoft.Extensions.DependencyInjection;
using PinoyCleanArch;

namespace PinoyTodo.Reader.Application;

public static class DependencyInjectionRegister
{
    public static IServiceCollection AddLocalApplication(this IServiceCollection services)
    {
        services.AddApplication(AppDomain.CurrentDomain.GetAssemblies());

        return services;
    }
}