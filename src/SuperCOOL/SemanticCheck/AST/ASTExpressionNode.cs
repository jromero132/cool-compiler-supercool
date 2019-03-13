using SuperCOOL.Core;

namespace SuperCOOL.SemanticCheck.AST
{
    public abstract class ASTExpressionNode:ASTNode
    {
        public override Result Accept<Result>(ISuperCoolASTVisitor<Result> Visitor)
        {
            return Visitor.VisitExpression(this);
        }
    }
}