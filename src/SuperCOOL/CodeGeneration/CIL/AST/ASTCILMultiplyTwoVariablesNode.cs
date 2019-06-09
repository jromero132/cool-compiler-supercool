using SuperCOOL.Constants;

namespace SuperCOOL.CodeGeneration.CIL.AST
{
  public class ASTCILMultiplyTwoVariablesNode : ASTCILExpressionNode
    {
        public ASTCILExpressionNode Left { get; }
        public ASTCILExpressionNode Right { get; }

        public ASTCILMultiplyTwoVariablesNode(ASTCILExpressionNode left, ASTCILExpressionNode right) : base(Types.Int)
        {
            Left = left;
            Right = right;
        }
    }
}
