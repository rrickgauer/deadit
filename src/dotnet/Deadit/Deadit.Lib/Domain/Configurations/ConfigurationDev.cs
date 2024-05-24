namespace Deadit.Lib.Domain.Configurations;

public class ConfigurationDev : ConfigurationProduction, IConfigs
{
    public override bool IsProduction => false;

    public override string DbDataBase => "Deadit_Dev";

    public override string StaticWebFilesPath => @"C:\xampp\htdocs\files\deadit\src\dotnet\Deadit\Deadit.WebGui\wwwroot";
}

