using SuperCOOL.Core;

namespace SuperCOOL.SemanticCheck.AST
{
    public class ASTStringConstantNode : ASTExpressionNode
    {
        public string Value { get; set; }
        public override Result Accept<Result>(ISuperCoolASTVisitor<Result> Visitor)
        {
            return Visitor.VisitStringConstant(this);
        }
    }
}