namespace QueryBuilder.Lexer
{
    public readonly struct Input
    {
        private string Source { get; }

        private int Length { get; }

        public Position Position { get; }

        public Input(string source)
            : this(source, Position.Start, source.Length)
        {
        }

        private Input(string source, Position position, int length)
        {
            Source = source;
            Length = length;
            Position = position;
        }

        public Result<char> NextChar()
        {
            if (Length == 0)
            {
                return Result.Empty<char>(this);
            }

            var @char = Source[Position.Absolute];
            return Result.Value(@char, new Input(Source, Position.MovePointer(@char), Length - 1));
        }
    }
}