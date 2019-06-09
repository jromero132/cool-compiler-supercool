using SuperCOOL.Constants;

namespace SuperCOOL.CodeGeneration.CIL.AST
{
    public class ASTCILStringConstantNode : ASTCILExpressionNode
    {
        public string Value { get; }
        public string DataLabel { get; }

        public ASTCILStringConstantNode( string value, Core.ISymbolTable symbolTable, string dataLabel) : base(symbolTable )
        {
            Value = value;
            DataLabel = dataLabel;
        }

        public override Result Accept<Result>( ICILVisitor<Result> Visitor ) => Visitor.VisitStringConstant( this );
    }
}
