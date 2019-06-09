namespace SuperCOOL.CodeGeneration.CIL.AST
{
    public class ASTCILAllocateNode : ASTCILExpressionNode
    {
        public ASTCILAllocateNode( string type ) : base( type )
        {
        }

        public override Result Accept<Result>( ICILVisitor<Result> Visitor ) => Visitor.VisitAllocate( this );
    }
}
