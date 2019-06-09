namespace SuperCOOL.CodeGeneration.CIL.AST
{
    public abstract class ASTCILNode
    {
        public virtual Result Accept<Result>( ICILVisitor<Result> Visitor ) => Visitor.VisitNode( this );
    }
}
