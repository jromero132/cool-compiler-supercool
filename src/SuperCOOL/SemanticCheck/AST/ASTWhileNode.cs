using SuperCOOL.Core;

namespace SuperCOOL.SemanticCheck.AST
{
    public class ASTWhileNode : ASTExpressionNode
    {
        public ASTExpressionNode Condition { get; set; }
        public ASTExpressionNode Body { get; set; }

        public override Result Accept<Result>(ISuperCoolASTVisitor<Result> Visitor)
        {
            return Visitor.VisitWhile(this);
        }
    }
}