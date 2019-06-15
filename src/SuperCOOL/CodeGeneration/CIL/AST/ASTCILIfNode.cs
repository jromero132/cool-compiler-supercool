using SuperCOOL.Core;

namespace SuperCOOL.CodeGeneration.CIL.AST
{
    public class ASTCILIfNode : ASTCILExpressionNode
    {
        public ASTCILExpressionNode Condition { get; }
        public ASTCILExpressionNode Then { get; }
        public ASTCILExpressionNode Else { get; }
        public string EndLabel { get; }
        public string ElseLabel { get; }
        public string IfLabel { get; }

        public ASTCILIfNode( ASTCILExpressionNode condition, ASTCILExpressionNode then, ASTCILExpressionNode @else,
            (string endLabel, string elseLabel, string ifLabel) labels) : base()
        {
            Condition = condition;
            Then = then;
            Else = @else;
            EndLabel = labels.endLabel;
            ElseLabel = labels.elseLabel;
            IfLabel = labels.ifLabel;
        }

        public override Result Accept<Result>( ICILVisitor<Result> Visitor ) => Visitor.VisitIf( this );
    }
}
