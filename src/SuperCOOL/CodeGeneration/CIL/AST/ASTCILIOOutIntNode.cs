using System.Linq;
using SuperCOOL.Constants;
using SuperCOOL.Core;

namespace SuperCOOL.CodeGeneration.CIL.AST
{
    public class ASTCILIOOutIntNode : ASTCILFuncNode
    {
        public ASTCILIOOutIntNode(ILabelILGenerator labelIlGenerator, ISymbolTable symbolTable) : base(
            labelIlGenerator.GenerateFunc(Types.IO, Functions.OutInt),
            Enumerable.Empty<ASTCILExpressionNode>(), symbolTable)
        {
        }

        public override Result Accept<Result>(ICILVisitor<Result> Visitor) => Visitor.VisitIOOutInt(this);
    }
}
