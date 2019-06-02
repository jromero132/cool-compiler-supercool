namespace SuperCOOL.CodeGeneration.CIL.AST
{
    class ASTCILIsVoidNode : ASTCILExpressionNode
    {
        public ASTCILExpressionNode Expression { get; }

        public ASTCILIsVoidNode(ASTCILExpressionNode expression)
        {
            Expression = expression;
        }
    }
}
