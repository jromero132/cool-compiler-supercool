using SuperCOOL.Core;

namespace SuperCOOL.CodeGeneration.CIL.AST
{
    public class ASTCILUnboxingNode : ASTCILExpressionNode
    {
        public ASTCILExpressionNode Expression { get; }
        public CoolType Type { get; }
        public ASTCILUnboxingNode(ASTCILExpressionNode expression, CoolType type) :base()
        {
            this.Expression = expression;
            this.Type = type;
        }
        public override Result Accept<Result>( ICILVisitor<Result> Visitor ) => Visitor.VisitUnboxing( this );
    }
}
