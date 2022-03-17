using QueryBuilder.Core.Enums;

namespace QueryBuilder.Core.Models
{
    public class Token
    {
        public TokenType TokenType { get; set; }

        public string Lexeme { get; set; }

        public int Line { get; set; }

        public int Column { get; set; }

        public override string ToString()
        {
            return $"Lexeme: {Lexeme}, type: {TokenType} found in line: {Line}, column: {Column}";
        }

        public static implicit operator TokenType(Token token) => token.TokenType;
    }
}