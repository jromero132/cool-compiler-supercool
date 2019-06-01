namespace SuperCOOL.CodeGeneration.CIL.AST
{
    class ASTCILDivideTwoConstantNode : ASTCILExpressionNode
    {
        public int Left { get; }
        public int Right { get; }

        public ASTCILDivideTwoConstantNode(int left, int right)
        {
            Left = left;
            Right = right;
        }
    }
}
