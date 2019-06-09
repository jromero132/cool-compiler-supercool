namespace SuperCOOL.CodeGeneration.CIL.AST
{
    public class ASTCILVoidNode : ASTCILExpressionNode
    {
        public override Result Accept<Result>( ICILVisitor<Result> Visitor ) => Visitor.VisitVoid( this );
    }
}
