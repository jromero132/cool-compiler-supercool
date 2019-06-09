using SuperCOOL.Constants;

namespace SuperCOOL.CodeGeneration.CIL.AST
{
    public class ASTCILStringConstantNode : ASTCILExpressionNode
    {
        public string Value { get; }
        public string DataLabel { get; }

        public ASTCILStringConstantNode( string value, string dataLabel ) : base( Types.String )
        {
            Value = value;
            DataLabel = dataLabel;
        }

        public override Result Accept<Result>( ICILVisitor<Result> Visitor ) => Visitor.VisitStringConstant( this );
    }
}
