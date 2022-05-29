using Microsoft.Extensions.DependencyInjection;

namespace TeamContactTelegramBot.CoreSystem
{
    public interface ITeamContactDI
    {
    }

    public static class ITemContactDIExtensions
    {
        private static IServiceProvider _serviceProvider;

        public static void SerProvider(IServiceProvider serviceProvider)
        {
            _serviceProvider = serviceProvider;
        }

        public static object GetService(Type serviceInterfaceType)
        {
            IServiceProvider serviceProvider = _serviceProvider.GetService(typeof(IServiceProvider)) as IServiceProvider;
            return serviceProvider.GetRequiredService(serviceInterfaceType);
        }

        public static T GetService<T>() where T : class
        {
            return (T) GetService(typeof(T));
        }

        public static T GetService<T>(this ITeamContactDI _) where T : class
        {
            return GetService<T>();
        }
    }
}
