namespace SuperCOOL.CodeGeneration.CIL.AST
{
    public class ASTCILIfNode : ASTCILExpressionNode
    {
        public ASTCILExpressionNode Condition { get; }
        public ASTCILExpressionNode Then { get; }
        public ASTCILExpressionNode Else { get; }
        public string EndLabel { get; }
        public string ElseLabel { get; }

        public ASTCILIfNode( ASTCILExpressionNode condition, ASTCILExpressionNode then, ASTCILExpressionNode @else,
            (string endLabel, string elseLabel) labels )
        {
            Condition = condition;
            Then = then;
            Else = @else;
            EndLabel = labels.endLabel;
            ElseLabel = labels.elseLabel;
        }

        public override Result Accept<Result>( ICILVisitor<Result> Visitor ) => Visitor.VisitIf( this );
    }
}
