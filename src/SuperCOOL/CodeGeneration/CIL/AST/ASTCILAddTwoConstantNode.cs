namespace SuperCOOL.CodeGeneration.CIL.AST
{
    class ASTCILAddTwoConstantNode : ASTCILExpressionNode
    {
        public int Left { get; }
        public int Right { get; }

        public ASTCILAddTwoConstantNode(int left, int right)
        {
            Left = left;
            Right = right;
        }
    }
}
