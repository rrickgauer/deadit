using Deadit.Lib.Domain.Attributes;
using Deadit.Lib.Domain.Enum;
using System.Reflection;

namespace Deadit.WebGui;

public class WebGuiSetup
{

    public static void InjectServicesIntoAssembly(IServiceCollection services, InjectionProject projectType, Assembly assembly)
    {
        var serviceTypes = assembly.GetTypes().Where(t => t.IsClass && t.GetCustomAttribute<AutoInjectAttribute>() != null).ToList() ?? new List<Type>();

        foreach (var serviceType in serviceTypes)
        {
            InjectService(services, projectType, serviceType);
        }
    }

    private static void InjectService(IServiceCollection services, InjectionProject project, Type serviceType)
    {
        var attr = serviceType.GetCustomAttribute<AutoInjectAttribute>();

        if (attr == null)
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

    private static Func<Type, IServiceCollection> GetInjectionMethod(IServiceCollection services, AutoInjectAttribute attr)
    {
        return attr.AutoInjectionType switch
        {
            AutoInjectionType.Singleton => services.AddSingleton,
            AutoInjectionType.Scoped    => services.AddScoped,
            AutoInjectionType.Transient => services.AddTransient,
            _                           => throw new NotImplementedException(),
        };
    }

    private static Func<Type, Type, IServiceCollection> GetInterfaceInjectionMethod(IServiceCollection services, AutoInjectAttribute attr)
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
