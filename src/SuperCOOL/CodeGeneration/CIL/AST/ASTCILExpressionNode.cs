using SuperCOOL.Core;

namespace SuperCOOL.CodeGeneration.CIL.AST
{
    public abstract class ASTCILExpressionNode : ASTCILNode
    {
        protected ASTCILExpressionNode()
        {
        }

        public override Result Accept<Result>( ICILVisitor<Result> Visitor ) => Visitor.VisitExpression( this );
    }
}
