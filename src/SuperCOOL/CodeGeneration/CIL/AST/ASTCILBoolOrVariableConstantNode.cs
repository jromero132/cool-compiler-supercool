using SuperCOOL.Core;

namespace SuperCOOL.CodeGeneration.CIL.AST
{
    public class ASTCILBoolOrVariableConstantNode : ASTCILExpressionNode
    {
        public ASTCILExpressionNode Left { get; }
        public bool Right { get; }

        public ASTCILBoolOrVariableConstantNode( ASTCILExpressionNode left, bool right,ISymbolTable symbolTable):base(symbolTable)
        {
            Left = left;
            Right = right;
        }

        public override Result Accept<Result>( ICILVisitor<Result> Visitor ) => Visitor.VisitBoolOrVariableConstant( this );
    }
}
