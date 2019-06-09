namespace SuperCOOL.CodeGeneration.CIL.AST
{
    public class ASTCILDivideVariableConstantNode : ASTCILExpressionNode
    {
        public ASTCILExpressionNode Left { get; }
        public int Right { get; }

        public ASTCILDivideVariableConstantNode( ASTCILExpressionNode left, int right )
        {
            Left = left;
            Right = right;
        }

        public override Result Accept<Result>( ICILVisitor<Result> Visitor ) => Visitor.VisitDivideVariableConstant( this );
    }
}
