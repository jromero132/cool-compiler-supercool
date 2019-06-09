using SuperCOOL.Constants;

namespace SuperCOOL.CodeGeneration.CIL.AST
{
    public class ASTCILIOOutStringNode : ASTCILFuncNode
    {
        public ASTCILIOOutStringNode( ILabelILGenerator labelIlGenerator, ASTCILExpressionNode expresion ) : base( labelIlGenerator.GenerateFunc( Types.IO, Functions.OutString ),
            new[] { expresion } )
        {
        }

        public override Result Accept<Result>( ICILVisitor<Result> Visitor ) => Visitor.VisitIOOutString( this );
    }
}
