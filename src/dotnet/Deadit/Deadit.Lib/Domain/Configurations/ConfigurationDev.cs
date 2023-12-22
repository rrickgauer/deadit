namespace Deadit.Lib.Domain.Configurations;

public class ConfigurationDev : ConfigurationProduction, IConfigs
{
    public override bool IsProduction => false;

    public override string DbDataBase => "Deadit_Dev";
}
