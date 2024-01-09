namespace Deadit.Lib.Domain.Enum;


[Flags]
public enum InjectionProject
{
    None   = 0,
    Always = 1 << 0,
    WebGui = 1 << 1,
    Api    = 1 << 2,
}



public enum AutoInjectionType
{
    Singleton,
    Scoped,
    Transient,
}

