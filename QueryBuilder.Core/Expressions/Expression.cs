using QueryBuilder.Core.Models;

namespace QueryBuilder.Core.Expressions
{
    public abstract class Expression : Node
    {
        protected readonly CompilerType CompilerType;

        public Token Token { get; }

        public Expression(Token token, CompilerType compilerType)
        {
            Token = token;
            this.CompilerType = compilerType;
        }

        public abstract string GenerateCode();
    }
}