using Antlr4.Runtime;
using SuperCOOL.Core;

namespace SuperCOOL.SemanticCheck.AST
{
    public class ASTLetInNode : ASTExpressionNode
    {
        public (IToken Id, IToken Type, ASTExpressionNode Expression)[] Declarations { get; internal set; }
        public ASTExpressionNode LetExp { get; internal set; }
        public override Result Accept<Result>(ISuperCoolASTVisitor<Result> Visitor)
        {
            return Visitor.VisitLetIn(this);
        }
    }
}