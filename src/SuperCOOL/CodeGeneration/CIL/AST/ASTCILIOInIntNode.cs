using System.Linq;
using SuperCOOL.Constants;
using SuperCOOL.Core;

namespace SuperCOOL.CodeGeneration.CIL.AST
{
    public class ASTCILIOInIntNode : ASTCILFuncNode
    {
        public ASTCILIOInIntNode( ILabelILGenerator labelgenerator, ISymbolTable symbolTable ) : base( labelgenerator.GenerateFunc( Types.IO, Functions.InInt ),
            Enumerable.Empty<ASTCILExpressionNode>(), symbolTable )
        {
        }

        public override Result Accept<Result>( ICILVisitor<Result> Visitor ) => Visitor.VisitIOInInt( this );
    }
}
