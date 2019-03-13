using SuperCOOL.Core;

namespace SuperCOOL.SemanticCheck.AST
{
    public class ASTMultiplyNode : ASTExpressionNode
    {
        public ASTExpressionNode Left { get; set; }
        public ASTExpressionNode Right { get; set; }

        public override Result Accept<Result>(ISuperCoolASTVisitor<Result> Visitor)
        {
            return Visitor.VisitMultiply(this);
        }
    }
}