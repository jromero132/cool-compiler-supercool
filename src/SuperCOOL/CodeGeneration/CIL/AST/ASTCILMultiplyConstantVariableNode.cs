namespace SuperCOOL.CodeGeneration.CIL.AST
{
    class ASTCILMultiplyConstantVariableNode : ASTCILExpressionNode
    {
        public int Left { get; }
        public ASTCILExpressionNode Right { get; }

        public ASTCILMultiplyConstantVariableNode(int left, ASTCILExpressionNode right)
        {
            Left = left;
            Right = right;
        }
    }
}
