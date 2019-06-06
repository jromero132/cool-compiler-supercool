using Antlr4.Runtime;
using SuperCOOL.Core;

namespace SuperCOOL.SemanticCheck.AST
{
    public class ASTIfNode : ASTExpressionNode
    {
        public ASTExpressionNode Condition { get; set; }
        public ASTExpressionNode Then { get; set; }
        public ASTExpressionNode Else { get; set; }
        public IToken IfToken { get; internal set; }

        public override Result Accept<Result>(ISuperCoolASTVisitor<Result> Visitor)
        {
            return Visitor.VisitIf(this);
        }
    }
}