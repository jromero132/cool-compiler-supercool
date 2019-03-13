using SuperCOOL.Core;

namespace SuperCOOL.SemanticCheck.AST
{
    public class ASTBlockNode : ASTExpressionNode
    {
        public ASTExpressionNode[] Expresions { get; internal set; }
        public override Result Accept<Result>(ISuperCoolASTVisitor<Result> Visitor)
        {
            return Visitor.VisitBlock(this);
        }
    }
}