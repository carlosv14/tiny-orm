using QueryBuilder.Core.Models;

namespace QueryBuilder.Core.Interfaces
{
    public interface IScanner
    {
        Token GetNextToken();
    }   
}