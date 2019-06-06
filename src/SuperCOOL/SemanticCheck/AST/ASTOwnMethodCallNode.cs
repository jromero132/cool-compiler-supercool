using Antlr4.Runtime;
using SuperCOOL.Core;

namespace SuperCOOL.SemanticCheck.AST
{
    public class ASTOwnMethodCallNode : ASTExpressionNode
    {
        public IToken Method { get; set; }
        public string MethodName => Method.Text;
        public ASTExpressionNode[] Arguments { get; set; }

        public override Result Accept<Result>(ISuperCoolASTVisitor<Result> Visitor)
        {
            return Visitor.VisitOwnMethodCall(this);
        }
    }
}