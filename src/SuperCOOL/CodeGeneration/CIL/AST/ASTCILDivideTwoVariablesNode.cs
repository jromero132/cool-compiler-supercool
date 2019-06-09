namespace SuperCOOL.CodeGeneration.CIL.AST
{
    public class ASTCILDivideTwoVariablesNode : ASTCILExpressionNode
    {
        public ASTCILExpressionNode Left { get; }
        public ASTCILExpressionNode Right { get; }

        public ASTCILDivideTwoVariablesNode( ASTCILExpressionNode left, ASTCILExpressionNode right )
        {
            Left = left;
            Right = right;
        }

        public override Result Accept<Result>( ICILVisitor<Result> Visitor ) => Visitor.VisitDivideTwoVariables( this );
    }
}
