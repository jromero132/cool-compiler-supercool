using SuperCOOL.Constants;

namespace SuperCOOL.CodeGeneration.CIL.AST
{
  public class ASTCILMultiplyVariableConstantNode : ASTCILExpressionNode
    {
        public ASTCILExpressionNode Left { get; }
        public int Right { get; }

        public ASTCILMultiplyVariableConstantNode(ASTCILExpressionNode left, int right) : base(Types.Int)
        {
            Left = left;
            Right = right;
        }
    }
}
