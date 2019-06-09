namespace SuperCOOL.CodeGeneration.CIL.AST
{
    public class ASTCILIdNode : ASTCILNode
    {
        public string Name { get; }

        public ASTCILIdNode( string name )
        {
            Name = name;
        }

        public override Result Accept<Result>( ICILVisitor<Result> Visitor ) => Visitor.VisitId( this );
    }
}
