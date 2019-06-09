namespace SuperCOOL.CodeGeneration.CIL.AST
{
    public class ASTCILNewNode : ASTCILExpressionNode
    {

        public ASTCILNewNode( string type ) : base( type )
        {
        }

        public override Result Accept<Result>( ICILVisitor<Result> Visitor ) => Visitor.VisitNew( this );
    }
}
