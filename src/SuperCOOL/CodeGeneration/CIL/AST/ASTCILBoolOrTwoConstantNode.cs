namespace SuperCOOL.CodeGeneration.CIL.AST
{
    public class ASTCILBoolOrTwoConstantNode : ASTCILExpressionNode
    {
        public bool Left { get; }
        public bool Right { get; }

        public ASTCILBoolOrTwoConstantNode(bool left, bool right)
        {
            Left = left;
            Right = right;
        }
    }
}
