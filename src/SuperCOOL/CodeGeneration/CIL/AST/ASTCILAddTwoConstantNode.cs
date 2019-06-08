using SuperCOOL.Constants;

namespace SuperCOOL.CodeGeneration.CIL.AST
{
    class ASTCILAddTwoConstantNode : ASTCILExpressionNode
    {
        public int Left { get; }
        public int Right { get; }

        public ASTCILAddTwoConstantNode(int left, int right) : base(Types.Int)
        {
            Left = left;
            Right = right;
        }
    }
}
