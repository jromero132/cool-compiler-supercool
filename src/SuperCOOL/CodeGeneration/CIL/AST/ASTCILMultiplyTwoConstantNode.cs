namespace SuperCOOL.CodeGeneration.CIL.AST
{
    class ASTCILMultiplyTwoConstantNode : ASTCILExpressionNode
    {
        public int Left { get; }
        public int Right { get; }

        public ASTCILMultiplyTwoConstantNode(int left, int right)
        {
            Left = left;
            Right = right;
        }
    }
}
