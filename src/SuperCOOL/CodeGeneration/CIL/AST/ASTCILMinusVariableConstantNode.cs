namespace SuperCOOL.CodeGeneration.CIL.AST
{
    class ASTCILMinusVariableConstantNode : ASTCILExpressionNode
    {
        public ASTCILExpressionNode Left { get; }
        public int Right { get; }

        public ASTCILMinusVariableConstantNode(ASTCILExpressionNode left, int right)
        {
            Left = left;
            Right = right;
        }
    }
}
