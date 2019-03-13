using SuperCOOL.Core;

namespace SuperCOOL.SemanticCheck.AST
{
    public class ASTIntConstantNode : ASTExpressionNode
    {
        public int Value { get; set; }
        public override Result Accept<Result>(ISuperCoolASTVisitor<Result> Visitor)
        {
            return Visitor.VisitIntConstant(this);
        }
    }
}