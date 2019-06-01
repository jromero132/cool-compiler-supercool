namespace SuperCOOL.CodeGeneration.CIL.AST
{
    class ASTCILMinusTwoVariablesNode : ASTCILExpressionNode
    {
        public ASTCILExpressionNode Left { get; }
        public ASTCILExpressionNode Right { get; }

        public ASTCILMinusTwoVariablesNode(ASTCILExpressionNode left, ASTCILExpressionNode right)
        {
            Left = left;
            Right = right;
        }
    }
}
