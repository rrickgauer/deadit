using Deadit.Lib.Mapping.Tables;
using System.Data;

namespace Deadit.Lib.Service.Contracts;

public interface ITableMapperService
{
    public T ToModel<T>(DataRow dataRow);
    public List<T> ToModels<T>(DataTable dataTable);
    public TableMapper<T> GetMapper<T>();
}
