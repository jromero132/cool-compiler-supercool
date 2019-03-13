using SuperCOOL.Core;

namespace SuperCOOL.SemanticCheck.AST
{
    public class ASTAddNode : ASTExpressionNode
    {
        internal ASTExpressionNode Right { get; set; }
        internal ASTExpressionNode Left { get; set; }
        public override Result Accept<Result>(ISuperCoolASTVisitor<Result> Visitor)
        {
            return Visitor.VisitAdd(this);
        }
    }
}