namespace SuperCOOL.CodeGeneration.CIL.AST
{
    public class ASTCILBoolOrTwoVariablesNode : ASTCILExpressionNode
    {
        public ASTCILExpressionNode Left { get; }
        public ASTCILExpressionNode Right { get; }

        public ASTCILBoolOrTwoVariablesNode(ASTCILExpressionNode left, ASTCILExpressionNode right)
        {
            Left = left;
            Right = right;
        }
    }
}
