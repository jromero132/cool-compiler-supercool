using Antlr4.Runtime;
using SuperCOOL.Core;

namespace SuperCOOL.SemanticCheck.AST
{
    public class ASTStaticMethodCallNode:ASTExpressionNode
    {
        public IToken Method { get; set; }
        public string MethodName => Method.Text;
        public IToken Type { get; set; }
        public string TypeName => Type.Text;
        internal ASTExpressionNode InvokeOnExpresion { get; set; }
        internal ASTExpressionNode[] Arguments { get; set; }

        public override Result Accept<Result>(ISuperCoolASTVisitor<Result> Visitor)
        {
            return Visitor.VisitStaticMethodCall(this);
        }
    }
}