using SuperCOOL.Constants;
using SuperCOOL.Core;
using System.Linq;

namespace SuperCOOL.CodeGeneration.CIL.AST
{
    public class ASTCILStringSubStrNode : ASTCILFuncNode
    {
        public ASTCILStringSubStrNode(ILabelILGenerator labelIlGenerator, ISymbolTable symbolTable) : base(
            labelIlGenerator.GenerateFunc(Types.String, Functions.Substr), Enumerable.Empty<ASTCILExpressionNode>(),
            symbolTable)
        {
        }

        public override Result Accept<Result>(ICILVisitor<Result> Visitor)
        {
            return Visitor.VisitStringSubStr(this);
        }
    }
}
