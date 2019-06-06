
using SuperCOOL.Core;

namespace SuperCOOL.SemanticCheck.AST
{
    public class ASTBoolConstantNode : ASTExpressionNode
    {
        public bool Value { get; internal set; }

        public override Result Accept<Result>(ISuperCoolASTVisitor<Result> Visitor)
        {
            return Visitor.VisitBoolConstant(this);
        }
    }
}