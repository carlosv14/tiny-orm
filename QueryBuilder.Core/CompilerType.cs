using QueryBuilder.Core.Enums;
using System;

namespace QueryBuilder.Core
{
    public class CompilerType : IEquatable<CompilerType>
    {
        private string Lexeme { get; }

        private TokenType TokenType { get; }

        public CompilerType(string lexeme, TokenType tokenType)
        {
            Lexeme = lexeme;
            TokenType = tokenType;
        }

        public bool Equals(CompilerType other)
        {
            if (ReferenceEquals(null, other)) return false;
            if (ReferenceEquals(this, other)) return true;

            return Lexeme == other.Lexeme && TokenType == other.TokenType;
        }

        public override bool Equals(object obj)
        {
            if (ReferenceEquals(null, obj)) return false;
            if (ReferenceEquals(this, obj)) return true;

            if (obj.GetType() != this.GetType())
            {
                return false;
            }

            return Equals((CompilerType)obj);
        }

        public override int GetHashCode()
        {
            return Tuple.Create(Lexeme, TokenType).GetHashCode();
        }

        public override string ToString()
        {
            return Lexeme;
        }

        public static bool operator ==(CompilerType a, CompilerType b) => a.Equals(b);

        public static bool operator !=(CompilerType a, CompilerType b) => !a.Equals(b);
    }
}