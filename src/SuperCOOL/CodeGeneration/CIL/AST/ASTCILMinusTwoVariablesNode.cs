using SuperCOOL.Constants;

namespace SuperCOOL.CodeGeneration.CIL.AST
{
    class ASTCILMinusTwoVariablesNode : ASTCILExpressionNode
    {
        public ASTCILExpressionNode Left { get; }
        public ASTCILExpressionNode Right { get; }

        public ASTCILMinusTwoVariablesNode(ASTCILExpressionNode left, ASTCILExpressionNode right) : base(Types.Int)
        {
            Left = left;
            Right = right;
        }
    }
}
