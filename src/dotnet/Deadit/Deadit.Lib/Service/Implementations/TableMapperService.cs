﻿using Deadit.Lib.Domain.Attributes;
using Deadit.Lib.Domain.Enum;
using Deadit.Lib.Mapping.Tables;
using Deadit.Lib.Service.Contracts;
using System.Data;
using System.Reflection;

namespace Deadit.Lib.Service.Implementations;

[AutoInject<ITableMapperService>(AutoInjectionType.Singleton, InjectionProject.Always)]
public class TableMapperService : ITableMapperService
{
    private static readonly List<object> ModelMappers = GetAllModelMappers().ToList();

    #region - Fetch all model mappers -

    /// <summary>
    /// Get a list of each model mapper instantiation
    /// </summary>
    /// <returns></returns>
    private static IEnumerable<object> GetAllModelMappers()
    {
        var subclassTypes = GetModelMapperSubclassTypes();

        var modelMapperObjects = subclassTypes.Select(Activator.CreateInstance);

        return modelMapperObjects.ToList();
    }

    /// <summary>
    /// Get each type of model mapper
    /// </summary>
    /// <returns></returns>
    private static IEnumerable<Type> GetModelMapperSubclassTypes()
    {
        Type modelMapperType = typeof(TableMapper<>);
        Assembly assembly = Assembly.GetExecutingAssembly(); // Or use the assembly containing the classes you want to inspect
        var subclasses = assembly.GetTypes().Where(t => IsSubclassOfGeneric(t, modelMapperType) && !t.IsAbstract);

        return subclasses;
    }

    /// <summary>
    /// Check if generic
    /// </summary>
    /// <param name="current"></param>
    /// <param name="genericBase"></param>
    /// <returns></returns>
    private static bool IsSubclassOfGeneric(Type current, Type genericBase)
    {
        do
        {
            if (current.IsGenericType && current.GetGenericTypeDefinition() == genericBase)
            {
                return true;
            }
        }
        while ((current = current.BaseType!) != null);

        return false;

    }

    #endregion


    public T ToModel<T>(DataRow dataRow)
    {
        return GetMapper<T>().ToModel(dataRow);
    }

    public List<T> ToModels<T>(DataTable dataTable)
    {
        return GetMapper<T>().ToModels(dataTable);
    }

    public TableMapper<T> GetMapper<T>()
    {
        TableMapper<T>? correctMapper = null;

        foreach (var mapper in ModelMappers)
        {
            if (mapper is TableMapper<T> castAttempt)
            {
                correctMapper = castAttempt;
                break;
            }
        }

        if (correctMapper == null)
        {
            throw new NotSupportedException($"Model type {typeof(T)} is not supported.");
        }

        return correctMapper;
    }

}
