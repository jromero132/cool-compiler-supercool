namespace SuperCOOL.CodeGeneration.CIL.AST
{
    public class ASTCILLocalNode : ASTCILExpressionNode
    {
        public string Name { get; }

        public ASTCILLocalNode( string name, string type ) : base( type )
        {
            Name = name;
        }

        public override Result Accept<Result>( ICILVisitor<Result> Visitor ) => Visitor.VisitLocal( this );
    }
}
