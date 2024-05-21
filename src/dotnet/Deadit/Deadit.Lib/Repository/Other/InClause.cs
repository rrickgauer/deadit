using MySql.Data.MySqlClient;

namespace Deadit.Lib.Repository.Other;

public class InClause<T>(IEnumerable<T> values)
{
    public List<T> Values { get; } = values.ToList();
    public Dictionary<string, T> ValuesDict { get; } = GetDict(values);

    private static Dictionary<string, T> GetDict(IEnumerable<T> values)
    {
        Dictionary<string, T> result = new();

        var valuesList = values.ToList();
        var count = valuesList.Count;

        for (int i = 0; i < count; i++)
        {
            string key = $"@in_clause_item_{i}";

            T value = valuesList[i];

            result.Add(key, value);
        }

        return result;
    }

    public string GetSqlClause()
    {
        string result = $"";

        bool isFirst = true;

        foreach (var key in ValuesDict.Keys)
        {
            if (isFirst)
            {
                result += $"{key}";
                isFirst = false;
            }

            else
            {
                result += $", {key}";
            }
        }

        return $"({result})";
    }


    public void AddParms(MySqlCommand command)
    {
        foreach (var item in ValuesDict)
        {
            command.Parameters.AddWithValue(item.Key, item.Value);
        }
    }

}
