using Deadit.Lib.Domain.Attributes;
using Deadit.Lib.Domain.Configurations;
using Deadit.Lib.Domain.Enum;
using Microsoft.Extensions.DependencyInjection;
using System.Reflection;

namespace Deadit.Lib.Utility;

public class DependencyInjectionUtilities
{

    public static void InjectConfigs(IServiceCollection services, bool isProduction)
    {
        if (isProduction)
        {
            services.AddSingleton<IConfigs, ConfigurationProduction>();
        }
        else
        {
            services.AddSingleton<IConfigs, ConfigurationDev>();
        }
    }



    public static void InjectServicesIntoAssembly(IServiceCollection services, InjectionProject projectType, Assembly assembly)
    {
        var serviceTypes = assembly.GetTypes().Where(t => t.IsClass && t.GetCustomAttribute<AutoInjectBaseAttribute>() != null).ToList() ?? new List<Type>();

        foreach (var serviceType in serviceTypes)
        {
            InjectService(services, projectType, serviceType);
        }
    }

    private static void InjectService(IServiceCollection services, InjectionProject project, Type serviceType)
    {
        if (serviceType.GetCustomAttribute<AutoInjectBaseAttribute>() is not AutoInjectBaseAttribute attr)
        {
            return;
        }

        if ((attr.Project & project) == 0)
        {
            return;
        }

        if (attr.InterfaceType != null)
        {
            GetInterfaceInjectionMethod(services, attr)(attr.InterfaceType, serviceType);
        }
        else
        {
            GetInjectionMethod(services, attr)(serviceType);
        }
    }

    private static Func<Type, IServiceCollection> GetInjectionMethod(IServiceCollection services, AutoInjectBaseAttribute attr)
    {
        return attr.AutoInjectionType switch
        {
            AutoInjectionType.Singleton => services.AddSingleton,
            AutoInjectionType.Scoped    => services.AddScoped,
            AutoInjectionType.Transient => services.AddTransient,
            _                           => throw new NotImplementedException(),
        };
    }

    private static Func<Type, Type, IServiceCollection> GetInterfaceInjectionMethod(IServiceCollection services, AutoInjectBaseAttribute attr)
    {
        return attr.AutoInjectionType switch
        {
            AutoInjectionType.Singleton => services.AddSingleton,
            AutoInjectionType.Scoped    => services.AddScoped,
            AutoInjectionType.Transient => services.AddTransient,
            _                           => throw new NotImplementedException(),
        };
    }

}
