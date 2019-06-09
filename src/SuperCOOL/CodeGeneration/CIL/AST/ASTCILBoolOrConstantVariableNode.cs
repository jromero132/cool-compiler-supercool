namespace SuperCOOL.CodeGeneration.CIL.AST
{
    public class ASTCILBoolOrConstantVariableNode : ASTCILExpressionNode
    {
        public bool Left { get; }
        public ASTCILExpressionNode Right { get; }

        public ASTCILBoolOrConstantVariableNode( bool left, ASTCILExpressionNode right )
        {
            Left = left;
            Right = right;
        }

        public override Result Accept<Result>( ICILVisitor<Result> Visitor ) => Visitor.VisitBoolOrConstantVariable( this );
    }
}
