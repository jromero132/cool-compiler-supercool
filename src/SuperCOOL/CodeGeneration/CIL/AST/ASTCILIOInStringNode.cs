using System.Linq;
using SuperCOOL.Constants;
using SuperCOOL.Core;

namespace SuperCOOL.CodeGeneration.CIL.AST
{
    public class ASTCILIOInStringNode : ASTCILFuncNode
    {
        public ASTCILIOInStringNode(CoolMethod method, ILabelILGenerator labelIlGenerator) : base( labelIlGenerator.GenerateFunc( Types.IO, Functions.InString ), method,
            Enumerable.Empty<ASTCILExpressionNode>() )
        {
        }

        public override Result Accept<Result>( ICILVisitor<Result> Visitor ) => Visitor.VisitIOInString( this );
    }
}
