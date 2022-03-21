using System;
using System.Collections.Generic;
using System.Linq;
using QueryBuilder.Core.Expressions;

namespace QueryBuilder.Core
{
    public static class ContextTable
    {
        private static readonly Dictionary<string, IList<IdExpression>> SymbolTable =
            new Dictionary<string, IList<IdExpression>>();

        /// <summary>
        /// Gets all the properties for a specific table.
        /// </summary>
        /// <param name="tableName"> Name of the table used in the statement.</param>
        /// <returns>A list of all the properties for the table provided.</returns>
        /// <exception cref="ApplicationException">If the table definition doesn't exist it will throw an exception.</exception>
        public static IEnumerable<IdExpression> GetAllProperties(string tableName)
        {
            var symbol = SymbolTable.TryGetValue(tableName, out var symbols) ? symbols : null;

            if (symbol == null)
            {
                throw new ApplicationException($"{tableName} not found in current context");
            }

            return symbol;
        }

        /// <summary>
        /// Finds a property for a specific table.
        /// </summary>
        /// <param name="tableName">Table where the property will be searched.</param>
        /// <param name="propertyLexeme">Name of the property being searched.</param>
        /// <returns>The property for the table and property name provided.</returns>
        /// <exception cref="ApplicationException">If the property is not found inside the table definition an exception is thrown.</exception>
        public static IdExpression GetProperty(string tableName, string propertyLexeme)
        {
            var symbol = SymbolTable.TryGetValue(tableName, out var symbols)
                ? symbols.FirstOrDefault(s => s.Token.Lexeme == propertyLexeme)
                : null;

            if (symbol == null)
            {
                throw new ApplicationException($"{propertyLexeme} not found in current context");
            }

            return symbol;
        }

        /// <summary>
        /// Adds a list of properties to a table.
        /// </summary>
        /// <param name="tableName">Name of the table where the properties will be added.</param>
        /// <param name="columns">List of properties to add.</param>
        /// <exception cref="ApplicationException">If the table already exists in the context an exception is thrown.</exception>
        public static void Add(string tableName, IList<IdExpression> columns)
        {
            if (SymbolTable.ContainsKey(tableName))
            {
                throw new ApplicationException($"Symbol {tableName} was previously defined in this scope");
            }
            SymbolTable.Add(tableName, columns);
        }

        /// <summary>
        /// Adds the table to the context.
        /// </summary>
        /// <param name="tableName">Name of the table to be added to the context.</param>
        /// <exception cref="ApplicationException">If the table already exists in the context an exception is thrown.</exception>
        public static void Add(string tableName)
        {
            if (SymbolTable.ContainsKey(tableName))
            {
                throw new ApplicationException($"Symbol {tableName} was previously defined in this scope");
            }
            SymbolTable.Add(tableName, new List<IdExpression>());
        }

        /// <summary>
        /// Adds one column to a table.
        /// </summary>
        /// <param name="tableName">Name of the table where the property will be added.</param>
        /// <param name="column">Property to add.</param>
        /// <exception cref="ApplicationException">If the table doesn't exists in the context an exception is thrown.</exception>
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