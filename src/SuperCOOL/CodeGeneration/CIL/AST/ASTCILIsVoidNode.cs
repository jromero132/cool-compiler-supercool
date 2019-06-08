using SuperCOOL.Constants;

namespace SuperCOOL.CodeGeneration.CIL.AST
{
    class ASTCILIsVoidNode : ASTCILExpressionNode
    {
        public ASTCILExpressionNode Expression { get; }

        public ASTCILIsVoidNode(ASTCILExpressionNode expression) : base(Types.Bool)
        {
            Expression = expression;
        }
    }
}
