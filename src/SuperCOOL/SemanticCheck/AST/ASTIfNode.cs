using SuperCOOL.Core;

namespace SuperCOOL.SemanticCheck.AST
{
    public class ASTIfNode : ASTExpressionNode
    {
        public ASTExpressionNode Condition { get; set; }
        public ASTExpressionNode Then { get; set; }
        public ASTExpressionNode Else { get; set; }
        public override Result Accept<Result>(ISuperCoolASTVisitor<Result> Visitor)
        {
            return Visitor.VisitIf(this);
        }
    }
}