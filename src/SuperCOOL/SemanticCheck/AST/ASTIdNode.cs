using SuperCOOL.Core;

namespace SuperCOOL.SemanticCheck.AST
{
    public class ASTIdNode : ASTExpressionNode
    {
        public string Name { get; set; }
        public override Result Accept<Result>(ISuperCoolASTVisitor<Result> Visitor)
        {
            return Visitor.VisitId(this);
        }
    }
}