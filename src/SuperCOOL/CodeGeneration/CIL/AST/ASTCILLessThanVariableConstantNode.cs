using SuperCOOL.Constants;

namespace SuperCOOL.CodeGeneration.CIL.AST
{
    public class ASTCILLessThanVariableConstantNode : ASTCILExpressionNode
    {
        public ASTCILExpressionNode Left { get; }
        public int Right { get; }

        public ASTCILLessThanVariableConstantNode(ASTCILExpressionNode left, int right) : base(Types.Bool)
        {
            Left = left;
            Right = right;
        }
    }
}
