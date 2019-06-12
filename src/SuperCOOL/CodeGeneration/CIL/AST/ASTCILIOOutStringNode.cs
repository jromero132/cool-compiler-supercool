using System.Linq;
using SuperCOOL.Constants;
using SuperCOOL.Core;

namespace SuperCOOL.CodeGeneration.CIL.AST
{
    public class ASTCILIOOutStringNode : ASTCILFuncNode
    {
        public ASTCILIOOutStringNode(CoolMethod method, ILabelILGenerator labelIlGenerator) : base( labelIlGenerator.GenerateFunc( Types.IO, Functions.OutString ), method,
            Enumerable.Empty<ASTCILExpressionNode>())
        {
        }

        public override Result Accept<Result>( ICILVisitor<Result> Visitor ) => Visitor.VisitIOOutString( this );
    }
}
