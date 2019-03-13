
using SuperCOOL.Core;

namespace SuperCOOL.SemanticCheck.AST
{
    public class ASTAtributeNode : ASTNode
    {
        public string Name { get; internal set; }
        public string Type { get; internal set; }
        public ASTExpressionNode Init { get; internal set; }

        public override Result Accept<Result>(ISuperCoolASTVisitor<Result> Visitor)
        {
            return Visitor.VisitAtribute(this);
        }
    }
}