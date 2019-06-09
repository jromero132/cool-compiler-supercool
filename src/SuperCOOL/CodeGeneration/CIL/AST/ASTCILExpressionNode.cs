using SuperCOOL.Core;

namespace SuperCOOL.CodeGeneration.CIL.AST
{
    public abstract class ASTCILExpressionNode : ASTCILNode
    {
        public ASTCILExpressionNode(ISymbolTable symbolTable):base(symbolTable)
        {
        }

        public override Result Accept<Result>( ICILVisitor<Result> Visitor ) => Visitor.VisitExpression( this );
    }
}
