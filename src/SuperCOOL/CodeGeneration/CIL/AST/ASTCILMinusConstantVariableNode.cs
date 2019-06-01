namespace SuperCOOL.CodeGeneration.CIL.AST
{
    class ASTCILMinusConstantVariableNode : ASTCILExpressionNode
    {
        public int Left { get; }
        public ASTCILExpressionNode Right { get; }

        public ASTCILMinusConstantVariableNode(int left, ASTCILExpressionNode right)
        {
            Left = left;
            Right = right;
        }
    }
}
