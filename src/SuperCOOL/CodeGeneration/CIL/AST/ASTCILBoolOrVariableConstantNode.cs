namespace SuperCOOL.CodeGeneration.CIL.AST
{
    public class ASTCILBoolOrVariableConstantNode : ASTCILExpressionNode
    {
        public ASTCILExpressionNode Left { get; }
        public bool Right { get; }

        public ASTCILBoolOrVariableConstantNode(ASTCILExpressionNode left, bool right)
        {
            Left = left;
            Right = right;
        }
    }
}
