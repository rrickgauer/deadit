namespace Deadit.WebGui.Controllers.Contracts;

public interface IControllerName
{
    private const string ControllerSuffix = "Controller";

    public static abstract string ControllerRedirectName { get; }

    public static string RemoveControllerSuffix(string controllerName)
    {
        string result = controllerName;

        if (controllerName.EndsWith(ControllerSuffix))
        {
            result = controllerName.Substring(0, controllerName.Length - ControllerSuffix.Length);
        }

        return result;
    }
}
