using SuperCOOL.Constants;

namespace SuperCOOL.CodeGeneration.CIL.AST
{
    public class ASTCILAddVariableConstantNode : ASTCILExpressionNode
    {
        public ASTCILExpressionNode Left { get; }
        public int Right { get; }

        public ASTCILAddVariableConstantNode(ASTCILExpressionNode left, int right) : base(Types.Int)
        {
            Left = left;
            Right = right;
        }
    }
}
