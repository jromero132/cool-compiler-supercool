namespace SuperCOOL.CodeGeneration.CIL.AST
{
    public class ASTCILAllocateNode : ASTCILExpressionNode
    {
        public ASTCILLocalNode Variable { get; }
        public ASTCILAllocateNode( string type, ASTCILLocalNode variable ) : base( type )
        {
            Variable = variable;
        }

        public override Result Accept<Result>( ICILVisitor<Result> Visitor ) => Visitor.VisitAllocate( this );
    }
}
