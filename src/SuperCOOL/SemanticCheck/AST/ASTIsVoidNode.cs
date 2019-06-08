using SuperCOOL.Core;

namespace SuperCOOL.SemanticCheck.AST
{
    public class ASTIsVoidNode : ASTExpressionNode
    {
        public ASTExpressionNode Expression { get; set; }

        public override Result Accept<Result>(ISuperCoolASTVisitor<Result> Visitor)
        {
            return Visitor.VisitIsvoid(this);
        }
    }
}