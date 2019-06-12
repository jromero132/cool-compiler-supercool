using SuperCOOL.Core;

namespace SuperCOOL.CodeGeneration.CIL.AST
{
    public class ASTCILVoidNode : ASTCILExpressionNode
    {
        public ASTCILVoidNode():base()
        {

        }
        public override Result Accept<Result>( ICILVisitor<Result> Visitor ) => Visitor.VisitVoid( this );
    }
}
