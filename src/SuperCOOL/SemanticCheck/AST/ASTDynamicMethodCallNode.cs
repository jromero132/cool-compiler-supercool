using Antlr4.Runtime;
using SuperCOOL.Core;

namespace SuperCOOL.SemanticCheck.AST
{
    public class ASTDynamicMethodCallNode:ASTExpressionNode
    {
        public IToken Method { get; set; }
        public string MethodName => Method.Text;
        internal ASTExpressionNode InvokeOnExpresion { get; set; }
        internal ASTExpressionNode[] Arguments { get; set; }

        public override Result Accept<Result>(ISuperCoolASTVisitor<Result> Visitor)
        {
            return Visitor.VisitDynamicMethodCall(this);
        }
    }
}