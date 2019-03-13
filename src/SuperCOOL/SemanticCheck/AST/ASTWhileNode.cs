using SuperCOOL.Core;

namespace SuperCOOL.SemanticCheck.AST
{
    public class ASTWhileNode : ASTExpressionNode
    {
        public ASTNode Condition { get; set; }
        public ASTNode Body { get; set; }

        public override Result Accept<Result>(ISuperCoolASTVisitor<Result> Visitor)
        {
            return Visitor.VisitWhile(this);
        }
    }
}