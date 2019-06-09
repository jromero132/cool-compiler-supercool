using SuperCOOL.Constants;

namespace SuperCOOL.CodeGeneration.CIL.AST
{
    public class ASTCILIntConstantNode : ASTCILExpressionNode
    {
        public int Value { get; }

        public ASTCILIntConstantNode( int value, Core.ISymbolTable symbolTable) : base(symbolTable)
        {
            Value = value;
        }

        public override Result Accept<Result>( ICILVisitor<Result> Visitor ) => Visitor.VisitIntConstant( this );
    }
}
