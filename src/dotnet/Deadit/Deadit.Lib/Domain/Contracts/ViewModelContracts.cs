using Deadit.Lib.Domain.Enum;

namespace Deadit.Lib.Domain.Contracts;

public class ViewModelContracts
{
}



public interface IClientIdArg
{
    public uint? ClientId { get; set; }
}


public interface ICommunityNameArg
{
    public string CommunityName { get; set; }
}

public interface IPostIdArg
{
    public Guid PostId { get; set; }
}

public interface ISortOptionArg
{
    public SortOption SortOption { get; set; }
}




