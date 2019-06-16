using SuperCOOL.Constants;

namespace SuperCOOL.CodeGeneration.CIL.AST
{
    public class ASTCILStringConstantNode : ASTCILExpressionNode
    {
        public string Value { get; }
        public string ValueLabel { get; }
        public string ObjectLabel { get; }

        public ASTCILStringConstantNode( string value, (string @object, string value) label) : base()
        {
            Value = value;
            ObjectLabel = label.@object;
            ValueLabel = label.value;
        }

        public override Result Accept<Result>( ICILVisitor<Result> Visitor ) => Visitor.VisitStringConstant( this );
    }
}
