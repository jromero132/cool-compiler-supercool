using Antlr4.Runtime;
using SuperCOOL.Core;

namespace SuperCOOL.SemanticCheck.AST
{
    public class ASTAddNode : ASTExpressionNode
    {
        public IToken AddToken { get; internal set; }
        internal ASTExpressionNode Right { get; set; }
        internal ASTExpressionNode Left { get; set; }
        public override Result Accept<Result>(ISuperCoolASTVisitor<Result> Visitor)
        {
            return Visitor.VisitAdd(this);
        }
    }
}