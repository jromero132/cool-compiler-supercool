using System.Runtime.InteropServices.ComTypes;

namespace SuperCOOL.CodeGeneration.CIL.AST
{
    public class ASTCILRuntimeErrorNode : ASTCILExpressionNode
    {
        public int Id { get; }
        public ASTCILRuntimeErrorNode( int id )
        {
            Id = id;
        }

        public override Result Accept<Result>( ICILVisitor<Result> Visitor ) => Visitor.VisitRuntimeError( this );
    }
}
