using SuperCOOL.Constants;

namespace SuperCOOL.CodeGeneration.CIL.AST
{
  public class ASTCILMultiplyConstantVariableNode : ASTCILExpressionNode
    {
        public int Left { get; }
        public ASTCILExpressionNode Right { get; }

        public ASTCILMultiplyConstantVariableNode(int left, ASTCILExpressionNode right) : base(Types.Int)
        {
            Left = left;
            Right = right;
        }
    }
}
