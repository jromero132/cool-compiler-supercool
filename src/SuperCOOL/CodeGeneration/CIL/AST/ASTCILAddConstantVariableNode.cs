namespace SuperCOOL.CodeGeneration.CIL.AST
{
    class ASTCILAddConstantVariableNode : ASTCILExpressionNode
    {
        public int Left { get; }
        public ASTCILExpressionNode Right { get; }

        public ASTCILAddConstantVariableNode(int left, ASTCILExpressionNode right)
        {
            Left = left;
            Right = right;
        }
    }
}
