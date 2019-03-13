using SuperCOOL.Core;

namespace SuperCOOL.SemanticCheck.AST
{
    public class ASTBoolNotNode : ASTExpressionNode
    {
        public ASTExpressionNode Expresion { get; set; }

        public override Result Accept<Result>(ISuperCoolASTVisitor<Result> Visitor)
        {
            return Visitor.VisitBoolNot(this);
        }
    }
}