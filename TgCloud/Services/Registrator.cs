using Microsoft.Extensions.DependencyInjection;

namespace TgCloud.Services
{
    public static class Registrator
    {
        public static IServiceCollection RegisterServices(this IServiceCollection services)
        {
            return services;
        }
    }
}
