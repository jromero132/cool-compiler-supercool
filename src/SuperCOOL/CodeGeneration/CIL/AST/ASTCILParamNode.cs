namespace SuperCOOL.CodeGeneration.CIL.AST
{
    public class ASTCILParamNode : ASTCILExpressionNode
    {
        public string Name { get; }

        public ASTCILParamNode( string name, string type ) : base( type )
        {
            Name = name;
        }

        public override Result Accept<Result>( ICILVisitor<Result> Visitor ) => Visitor.VisitParam( this );
    }
}
