namespace SuperCOOL.CodeGeneration.CIL.AST
{
    class ASTCILLessThanTwoConstantNode : ASTCILExpressionNode
    {
        public int Left { get; }
        public int Right { get; }

        public ASTCILLessThanTwoConstantNode(int left, int right)
        {
            Left = left;
            Right = right;
        }
    }
}
