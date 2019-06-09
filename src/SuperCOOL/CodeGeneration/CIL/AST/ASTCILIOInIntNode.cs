using System.Linq;
using SuperCOOL.Constants;

namespace SuperCOOL.CodeGeneration.CIL.AST
{
    public class ASTCILIOInIntNode : ASTCILFuncNode
    {
        public ASTCILIOInIntNode( ILabelILGenerator labelgenerator ) : base( labelgenerator.GenerateFunc( Types.IO, Functions.InInt ),
            Enumerable.Empty<ASTCILExpressionNode>() )
        {
        }

        public override Result Accept<Result>( ICILVisitor<Result> Visitor ) => Visitor.VisitIOInInt( this );
    }
}
