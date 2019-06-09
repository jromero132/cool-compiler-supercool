using System;
using System.Linq;
using SuperCOOL.Constants;

namespace SuperCOOL.CodeGeneration.CIL.AST
{
    public class ASTCILIOInStringNode : ASTCILFuncNode
    {
        public ASTCILIOInStringNode(ILabelILGenerator labelIlGenerator) : base(labelIlGenerator.GenerateFunc(Types.IO,Functions.InString),
            Enumerable.Empty<ASTCILExpressionNode>())
        {
            throw new NotImplementedException();
        }

        public override Result Accept<Result>( ICILVisitor<Result> Visitor ) => Visitor.VisitIOInString( this );
    }
}
