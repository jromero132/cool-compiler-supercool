using SuperCOOL.Core;

namespace SuperCOOL.SemanticCheck.AST
{
    public class ASTNegativeNode : ASTExpressionNode
    {
        public ASTNode Expression { get; set; }
        public override Result Accept<Result>(ISuperCoolASTVisitor<Result> Visitor)
        {
            return Visitor.VisitNegative(this);
        }
    }
}