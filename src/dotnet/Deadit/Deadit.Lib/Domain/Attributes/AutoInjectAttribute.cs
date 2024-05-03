using Deadit.Lib.Domain.Enum;

namespace Deadit.Lib.Domain.Attributes;



public abstract class AutoInjectBaseAttribute(AutoInjectionType autoInjectionType, InjectionProject project) : Attribute
{
    public AutoInjectionType AutoInjectionType { get; protected set; } = autoInjectionType;
    public InjectionProject Project { get; protected set; } = project;
    public abstract Type? InterfaceType { get; }
}

[AttributeUsage(AttributeTargets.Class, AllowMultiple = false)]
public class AutoInjectAttribute(AutoInjectionType autoInjectionType, InjectionProject project) : AutoInjectBaseAttribute(autoInjectionType, project)
{
    public override Type? InterfaceType => null;
}


[AttributeUsage(AttributeTargets.Class, AllowMultiple = false)]
public class AutoInjectAttribute<T>(AutoInjectionType autoInjectionType, InjectionProject project) : AutoInjectBaseAttribute(autoInjectionType, project)
{
    public override Type? InterfaceType => typeof(T);
}



//[AttributeUsage(AttributeTargets.Class, AllowMultiple = false)]
//public class AutoInjectAttribute : Attribute
//{
//    public AutoInjectionType AutoInjectionType { get; }
//    public InjectionProject Project { get; } = InjectionProject.Always;
//    public Type? InterfaceType { get; set; }

//    public AutoInjectAttribute(AutoInjectionType injectionType, InjectionProject project = InjectionProject.Always)
//    {
//        AutoInjectionType = injectionType;
//        Project = project;
//    }
//}











