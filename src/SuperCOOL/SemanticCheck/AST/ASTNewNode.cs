using SuperCOOL.Core;

namespace SuperCOOL.SemanticCheck.AST
{
    public class ASTNewNode:ASTExpressionNode
    {
        public string Type { get; internal set; }

        public override Result Accept<Result>(ISuperCoolASTVisitor<Result> Visitor)
        {
            return Visitor.VisitNew(this);
        }
    }
}