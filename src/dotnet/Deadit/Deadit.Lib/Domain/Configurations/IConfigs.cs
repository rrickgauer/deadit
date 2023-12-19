namespace Deadit.Lib.Domain.Configurations;

public interface IConfigs
{
    public bool IsProduction { get; }

    public string DbServer { get; }
    public string DbDataBase { get; }
    public string DbUser { get; }
    public string DbPassword { get; }
}
