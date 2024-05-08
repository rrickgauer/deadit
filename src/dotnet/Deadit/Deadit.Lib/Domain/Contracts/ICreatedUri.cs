namespace Deadit.Lib.Domain.Contracts;

public interface ICreatedUri
{
    public string UriWeb { get; }
    public string UriApi { get; }
    public string GetCreatedUri(string uriPrefix);
}
