using SuperCOOL.Constants;

namespace SuperCOOL.CodeGeneration.CIL.AST
{
    public class ASTCILIsVoidNode : ASTCILExpressionNode
    {
        public ASTCILExpressionNode Expression { get; }

        public ASTCILIsVoidNode(ASTCILExpressionNode expression) : base(Types.Bool)
        {
            Expression = expression;
        }
    }
}
