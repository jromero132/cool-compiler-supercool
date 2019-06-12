using SuperCOOL.Constants;

namespace SuperCOOL.CodeGeneration.CIL.AST
{
    public class ASTCILIntConstantNode : ASTCILExpressionNode
    {
        public int Value { get; }

        public ASTCILIntConstantNode( int value) : base()
        {
            Value = value;
        }

        public override Result Accept<Result>( ICILVisitor<Result> Visitor ) => Visitor.VisitIntConstant( this );
    }
}
