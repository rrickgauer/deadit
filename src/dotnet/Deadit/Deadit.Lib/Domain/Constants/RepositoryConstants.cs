namespace Deadit.Lib.Domain.Constants;

public class RepositoryConstants
{
    public const string PAGINATION_CLAUSE = @" LIMIT @pagination_limit OFFSET @pagination_offset ";

    public const int USER_DEFINED_EXCEPTION_NUMBER = 1644;
}
