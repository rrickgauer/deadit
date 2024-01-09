using Deadit.Lib.Domain.Enum;

namespace Deadit.Lib.Domain.Other;

public class AutoInject
{
    public Type InterfaceType { get; }
    public Type ImplementationType { get; }
    public AutoInjectionType InjectionType { get; }

    public AutoInject(Type interfaceType, Type implementationType, AutoInjectionType injectionType)
    {
        InterfaceType = interfaceType;
        ImplementationType = implementationType;
        InjectionType = injectionType;
    }
}
