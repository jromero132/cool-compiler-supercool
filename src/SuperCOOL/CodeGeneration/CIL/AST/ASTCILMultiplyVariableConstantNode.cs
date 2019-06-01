namespace SuperCOOL.CodeGeneration.CIL.AST
{
    class ASTCILMultiplyVariableConstantNode : ASTCILExpressionNode
    {
        public ASTCILExpressionNode Left { get; }
        public int Right { get; }

        public ASTCILMultiplyVariableConstantNode(ASTCILExpressionNode left, int right)
        {
            Left = left;
            Right = right;
        }
    }
}
