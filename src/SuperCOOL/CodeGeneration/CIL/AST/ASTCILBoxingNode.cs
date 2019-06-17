using SuperCOOL.Core;

namespace SuperCOOL.CodeGeneration.CIL.AST
{
    public class ASTCILBoxingNode : ASTCILExpressionNode
    {
        public ASTCILExpressionNode Expression { get; }
        public CoolType Type { get; }
        public ASTCILBoxingNode(ASTCILExpressionNode expression, CoolType type) :base()
        {
            this.Expression = expression;
            this.Type = type;
        }
        public override Result Accept<Result>( ICILVisitor<Result> Visitor ) => Visitor.VisitBoxing( this );
    }
}
