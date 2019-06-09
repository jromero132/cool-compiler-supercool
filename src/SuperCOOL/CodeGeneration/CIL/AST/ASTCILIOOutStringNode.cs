using System.Linq;
using SuperCOOL.Constants;
using SuperCOOL.Core;

namespace SuperCOOL.CodeGeneration.CIL.AST
{
    public class ASTCILIOOutStringNode : ASTCILFuncNode
    {
        public ASTCILIOOutStringNode( ILabelILGenerator labelIlGenerator, ISymbolTable symbolTable) : base( labelIlGenerator.GenerateFunc( Types.IO, Functions.OutString ),
            Enumerable.Empty<ASTCILExpressionNode>(), symbolTable)
        {
        }

        public override Result Accept<Result>( ICILVisitor<Result> Visitor ) => Visitor.VisitIOOutString( this );
    }
}
