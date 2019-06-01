namespace SuperCOOL.CodeGeneration.CIL.AST
{
    class ASTCILDivideVariableConstantNode : ASTCILExpressionNode
    {
        public ASTCILExpressionNode Left { get; }
        public int Right { get; }

        public ASTCILDivideVariableConstantNode(ASTCILExpressionNode left, int right)
        {
            Left = left;
            Right = right;
        }
    }
}
