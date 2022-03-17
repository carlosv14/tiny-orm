using QueryBuilder.Core.Models;

namespace QueryBuilder.Core.Expressions
{
    public class IdExpression : Expression
    {
        public IdExpression(Token token, CompilerType compilerType) : base(token, compilerType)
        {

        }

        public override string GenerateCode()
        {
            throw new System.NotImplementedException();
        }
    }
}