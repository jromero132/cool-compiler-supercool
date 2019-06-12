using SuperCOOL.Constants;
using SuperCOOL.Core;

namespace SuperCOOL.CodeGeneration.CIL.AST
{
    public class ASTCILMinusTwoVariablesNode : ASTCILExpressionNode
    {
        public ASTCILExpressionNode Left { get; }
        public ASTCILExpressionNode Right { get; }

        public ASTCILMinusTwoVariablesNode( ASTCILExpressionNode left, ASTCILExpressionNode right) : base()
        {
            Left = left;
            Right = right;
        }

        public override Result Accept<Result>( ICILVisitor<Result> Visitor ) => Visitor.VisitMinusTwoVariables( this );
    }
}
