using SuperCOOL.Core;

namespace SuperCOOL.SemanticCheck.AST
{
    public class ASTOwnMethodCallNode : ASTExpressionNode
    {
        public string Method { get; set; }
        public ASTExpressionNode[] Arguments { get; set; }

        public override Result Accept<Result>(ISuperCoolASTVisitor<Result> Visitor)
        {
            return Visitor.VisitOwnMethodCall(this);
        }
    }
}