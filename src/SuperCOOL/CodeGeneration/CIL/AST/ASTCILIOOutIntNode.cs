using SuperCOOL.Constants;

namespace SuperCOOL.CodeGeneration.CIL.AST
{
    public class ASTCILIOOutIntNode : ASTCILFuncNode
    {
        public ASTCILIOOutIntNode( ILabelILGenerator labelIlGenerator, ASTCILExpressionNode expresion ) : base( labelIlGenerator.GenerateFunc( Types.IO, Functions.OutInt ),
            new[] { expresion } )
        {
        }

        public override Result Accept<Result>( ICILVisitor<Result> Visitor ) => Visitor.VisitIOOutInt( this );
    }

}
