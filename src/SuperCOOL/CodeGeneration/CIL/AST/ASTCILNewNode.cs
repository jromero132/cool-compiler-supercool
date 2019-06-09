using SuperCOOL.Core;

namespace SuperCOOL.CodeGeneration.CIL.AST
{
    public class ASTCILNewNode : ASTCILExpressionNode
    {
        public string Type { get; }

        public ASTCILNewNode( string type, ISymbolTable symbolTable) : base( symbolTable )
        {
            Type = type;
        }

        public override Result Accept<Result>( ICILVisitor<Result> Visitor ) => Visitor.VisitNew( this );
    }
}
