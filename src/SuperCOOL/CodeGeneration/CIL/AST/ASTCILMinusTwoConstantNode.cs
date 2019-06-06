using SuperCOOL.Constants;

namespace SuperCOOL.CodeGeneration.CIL.AST
{
    class ASTCILMinusTwoConstantNode : ASTCILExpressionNode
    {
        public int Left { get; }
        public int Right { get; }

        public ASTCILMinusTwoConstantNode(int left, int right) : base(Types.Int)
        {
            Left = left;
            Right = right;
        }
    }
}
