
using SuperCOOL.Core;

namespace SuperCOOL.SemanticCheck.AST
{
    public class ASTBoolConstantNode : ASTNode
    {
        public bool Value { get; internal set; }

        public override Result Accept<Result>(ISuperCoolASTVisitor<Result> Visitor)
        {
            return Visitor.VisitBoolConstant(this);
        }
    }
}