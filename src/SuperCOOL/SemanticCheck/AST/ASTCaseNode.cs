using SuperCOOL.Core;

namespace SuperCOOL.SemanticCheck.AST
{
    public class ASTCaseNode : ASTExpressionNode
    {
        public ASTExpressionNode ExpressionCase { get; internal set; }
        public (string Name, string Type, ASTExpressionNode Branch)[] Cases { get; internal set; }

        public override Result Accept<Result>(ISuperCoolASTVisitor<Result> Visitor)
        {
            return Visitor.VisitCase(this);
        }
    }
}