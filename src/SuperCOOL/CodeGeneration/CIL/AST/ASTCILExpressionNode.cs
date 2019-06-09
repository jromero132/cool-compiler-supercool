namespace SuperCOOL.CodeGeneration.CIL.AST
{
    public abstract class ASTCILExpressionNode : ASTCILNode
    {
        public ASTCILExpressionNode()
        {

        }
        public ASTCILExpressionNode( string type )
        {
            Type = type;
        }
        public string Type { get; protected internal set; }

        public override Result Accept<Result>( ICILVisitor<Result> Visitor ) => Visitor.VisitExpression( this );
    }
}
