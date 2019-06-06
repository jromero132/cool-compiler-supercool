using Antlr4.Runtime;
using SuperCOOL.Core;

namespace SuperCOOL.SemanticCheck.AST
{
    public class ASTCaseNode : ASTExpressionNode
    {
        public ASTExpressionNode ExpressionCase { get; internal set; }
        public (IToken Name, IToken Type, ASTExpressionNode Branch)[] Cases { get; internal set; }

        public override Result Accept<Result>(ISuperCoolASTVisitor<Result> Visitor)
        {
            return Visitor.VisitCase(this);
        }
    }
}