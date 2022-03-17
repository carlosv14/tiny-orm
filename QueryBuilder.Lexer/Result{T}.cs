namespace QueryBuilder.Lexer
{
    public class Result<T>
    {
        public T Value { get; }

        public Input Reminder { get; }


        internal Result(T value, Input reminder)
        {
            Value = value;
            Reminder = reminder;
        }

        internal Result(Input reminder)
        {
            Reminder = reminder;
        }
    }
}