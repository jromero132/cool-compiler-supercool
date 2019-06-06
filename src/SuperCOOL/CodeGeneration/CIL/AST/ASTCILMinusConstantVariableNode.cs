using SuperCOOL.Constants;

namespace SuperCOOL.CodeGeneration.CIL.AST
{
    class ASTCILMinusConstantVariableNode : ASTCILExpressionNode
    {
        public int Left { get; }
        public ASTCILExpressionNode Right { get; }

        public ASTCILMinusConstantVariableNode(int left, ASTCILExpressionNode right) : base(Types.Int)
        {
            Left = left;
            Right = right;
        }
    }
}
