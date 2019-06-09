namespace SuperCOOL.CodeGeneration.CIL.AST
{
    public class ASTCILDivideConstantVariableNode : ASTCILExpressionNode
    {
        public int Left { get; }
        public ASTCILExpressionNode Right { get; }

        public ASTCILDivideConstantVariableNode(int left, ASTCILExpressionNode right)
        {
            Left = left;
            Right = right;
        }
    }
}
