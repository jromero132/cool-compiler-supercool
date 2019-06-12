using System.Linq;
using SuperCOOL.Constants;
using SuperCOOL.Core;

namespace SuperCOOL.CodeGeneration.CIL.AST
{
    public class ASTCILIOInIntNode : ASTCILFuncNode
    {
        public ASTCILIOInIntNode(CoolMethod coolMethod ,ILabelILGenerator labelgenerator ) : base( labelgenerator.GenerateFunc( Types.IO, Functions.InInt ), coolMethod,
            Enumerable.Empty<ASTCILExpressionNode>())
        {
        }

        public override Result Accept<Result>( ICILVisitor<Result> Visitor ) => Visitor.VisitIOInInt( this );
    }
}
