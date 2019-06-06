using Antlr4.Runtime;
using SuperCOOL.Core;

namespace SuperCOOL.SemanticCheck.AST
{
    public class ASTBoolNotNode : ASTExpressionNode
    {
        public ASTExpressionNode Expresion { get; set; }
        public IToken NotToken { get; internal set; }

        public override Result Accept<Result>(ISuperCoolASTVisitor<Result> Visitor)
        {
            return Visitor.VisitBoolNot(this);
        }
    }
}