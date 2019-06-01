namespace SuperCOOL.CodeGeneration.CIL.AST
{
    class ASTCILMinusTwoConstantNode : ASTCILExpressionNode
    {
        public int Left { get; }
        public int Right { get; }

        public ASTCILMinusTwoConstantNode(int left, int right)
        {
            Left = left;
            Right = right;
        }
    }
}
