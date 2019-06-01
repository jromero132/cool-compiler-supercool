namespace SuperCOOL.CodeGeneration.CIL.AST
{
    class ASTCILEqualNode : ASTCILExpressionNode
    {
        public ASTCILExpressionNode Left { get; }
        public ASTCILExpressionNode Right { get; }

        public ASTCILEqualNode(ASTCILExpressionNode left, ASTCILExpressionNode right)
        {
            Left = left;
            Right = right;
        }
    }
}
