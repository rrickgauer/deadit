using Deadit.Lib.Domain.Enum;
using MySql.Data.MySqlClient;

namespace Deadit.Lib.Domain.Errors;


public class RepositoryException : Exception
{
    public ErrorCode ErrorCode { get; }

    public RepositoryException(ErrorCode errorCode) : base()
    {
        ErrorCode = errorCode;
    }

    public RepositoryException(MySqlException ex) : base()
    {
        ErrorCode = (ErrorCode)(uint.Parse(ex.Message));
    }
}

