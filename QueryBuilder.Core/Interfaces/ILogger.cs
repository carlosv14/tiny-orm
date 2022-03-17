namespace QueryBuilder.Core.Interfaces
{
    public interface ILogger
    {
        void Error(string message);

        void Info(string message);
    }
}