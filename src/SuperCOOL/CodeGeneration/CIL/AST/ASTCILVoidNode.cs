using SuperCOOL.Core;

namespace SuperCOOL.CodeGeneration.CIL.AST
{
    public class ASTCILVoidNode : ASTCILExpressionNode
    {
        public ASTCILVoidNode(ISymbolTable symbolable):base(symbolable)
        {

        }
        public override Result Accept<Result>( ICILVisitor<Result> Visitor ) => Visitor.VisitVoid( this );
    }
}
