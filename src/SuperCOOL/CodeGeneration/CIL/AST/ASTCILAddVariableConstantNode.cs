namespace SuperCOOL.CodeGeneration.CIL.AST
{
    class ASTCILAddVariableConstantNode : ASTCILExpressionNode
    {
        public ASTCILExpressionNode Left { get; }
        public int Right { get; }

        public ASTCILAddVariableConstantNode(ASTCILExpressionNode left, int right)
        {
            Left = left;
            Right = right;
        }
    }
}
