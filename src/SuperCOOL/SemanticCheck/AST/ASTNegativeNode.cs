using Antlr4.Runtime;
using SuperCOOL.Core;

namespace SuperCOOL.SemanticCheck.AST
{
    public class ASTNegativeNode : ASTExpressionNode
    {
        public ASTNode Expression { get; set; }
        public IToken NegativeToken { get; internal set; }

        public override Result Accept<Result>(ISuperCoolASTVisitor<Result> Visitor)
        {
            return Visitor.VisitNegative(this);
        }
    }
}