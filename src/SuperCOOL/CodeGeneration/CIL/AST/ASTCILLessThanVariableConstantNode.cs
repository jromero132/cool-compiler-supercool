namespace SuperCOOL.CodeGeneration.CIL.AST
{
    class ASTCILLessThanVariableConstantNode : ASTCILExpressionNode
    {
        public ASTCILExpressionNode Left { get; }
        public int Right { get; }

        public ASTCILLessThanVariableConstantNode(ASTCILExpressionNode left, int right)
        {
            Left = left;
            Right = right;
        }
    }
}
