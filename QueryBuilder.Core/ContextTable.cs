using System;
using System.Collections.Generic;
using QueryBuilder.Core.Expressions;

namespace QueryBuilder.Core
{
    public static class ContextTable
    {
        private static readonly Dictionary<string, IList<IdExpression>> SymbolTable =
            new Dictionary<string, IList<IdExpression>>();

        public static IEnumerable<IdExpression> Get(string tableName)
        {
            if (SymbolTable.TryGetValue(tableName, out var symbol))
            {
                return symbol;
            }

            return null;
        }

        public static void Add(string tableName, IList<IdExpression> columns)
        {
            if (SymbolTable.ContainsKey(tableName))
            {
                throw new ApplicationException($"Symbol {tableName} was previously defined in this scope");
            }
            SymbolTable.Add(tableName, columns); 
        }
        
        public static void Add(string tableName)
        {
            if (SymbolTable.ContainsKey(tableName))
            {
                throw new ApplicationException($"Symbol {tableName} was previously defined in this scope");
            }
            SymbolTable.Add(tableName, new List<IdExpression>()); 
        }

        public static void Put(string tableName, IdExpression column)
        {
            if (!SymbolTable.ContainsKey(tableName))
            {
                throw new ApplicationException($"Symbol {tableName} hasn't been defined in this scope");
            }
            SymbolTable[tableName].Add(column);
        }
    }
}