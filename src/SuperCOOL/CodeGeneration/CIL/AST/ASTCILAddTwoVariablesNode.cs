namespace SuperCOOL.CodeGeneration.CIL.AST
{
    class ASTCILAddTwoVariablesNode : ASTCILExpressionNode
    {
        public ASTCILExpressionNode Left { get; }
        public ASTCILExpressionNode Right { get; }

        public ASTCILAddTwoVariablesNode(ASTCILExpressionNode left, ASTCILExpressionNode right)
        {
            Left = left;
            Right = right;
        }
    }
}
