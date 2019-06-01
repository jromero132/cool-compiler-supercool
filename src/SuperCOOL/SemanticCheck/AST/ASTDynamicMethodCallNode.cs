using SuperCOOL.Core;

namespace SuperCOOL.SemanticCheck.AST
{
    public class ASTDynamicMethodCallNode:ASTExpressionNode
    {
        public string MethodName { get; internal set; }
        internal ASTExpressionNode InvokeOnExpresion { get; set; }
        internal ASTExpressionNode[] Arguments { get; set; }

        public override Result Accept<Result>(ISuperCoolASTVisitor<Result> Visitor)
        {
            return Visitor.VisitDynamicMethodCall(this);
        }
    }
}