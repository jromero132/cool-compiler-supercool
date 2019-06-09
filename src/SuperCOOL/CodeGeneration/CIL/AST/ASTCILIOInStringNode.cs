using System.Linq;
using SuperCOOL.Constants;
using SuperCOOL.Core;

namespace SuperCOOL.CodeGeneration.CIL.AST
{
    public class ASTCILIOInStringNode : ASTCILFuncNode
    {
        public ASTCILIOInStringNode( ILabelILGenerator labelIlGenerator, ISymbolTable symbolTable ) : base( labelIlGenerator.GenerateFunc( Types.IO, Functions.InString ),
            Enumerable.Empty<ASTCILExpressionNode>(), symbolTable )
        {
        }

        public override Result Accept<Result>( ICILVisitor<Result> Visitor ) => Visitor.VisitIOInString( this );
    }
}
