namespace SuperCOOL.CodeGeneration.CIL.AST
{
    public class ASTCILEqualNode : ASTCILExpressionNode
    {
        public ASTCILExpressionNode Left { get; }
        public ASTCILExpressionNode Right { get; }

        public ASTCILEqualNode( ASTCILExpressionNode left, ASTCILExpressionNode right )
        {
            Left = left;
            Right = right;
        }

        public override Result Accept<Result>( ICILVisitor<Result> Visitor ) => Visitor.VisitEqual( this );
    }
}
