using SuperCOOL.Constants;
using SuperCOOL.Core;
using System.Linq;

namespace SuperCOOL.CodeGeneration.CIL.AST
{
    public class ASTCILStringConcatNode : ASTCILFuncNode
    {
        public ASTCILStringConcatNode(ILabelILGenerator labelIlGenerator, ISymbolTable symbolTable) : base(
            labelIlGenerator.GenerateFunc(Types.String, Functions.Concat), Enumerable.Empty<ASTCILExpressionNode>(),
            symbolTable)
        {
        }

        public override Result Accept<Result>(ICILVisitor<Result> Visitor)
        {
            return Visitor.VisitStringConcat(this);
        }
    }
}
