using SuperCOOL.Constants;

namespace SuperCOOL.CodeGeneration.CIL.AST
{
    public class ASTCILAddTwoVariablesNode : ASTCILExpressionNode
    {
        public ASTCILExpressionNode Left { get; }
        public ASTCILExpressionNode Right { get; }

        public ASTCILAddTwoVariablesNode(ASTCILExpressionNode left, ASTCILExpressionNode right) :base(Types.Int)
        {
            Left = left;
            Right = right;
        }
    }
}
