using SuperCOOL.Constants;
using SuperCOOL.Core;
using System.Linq;

namespace SuperCOOL.CodeGeneration.CIL.AST
{
    class ASTCILStringSubStrNode : ASTCILFuncNode
    {
        public ASTCILStringSubStrNode(ILabelILGenerator labelIlGenerator, ISymbolTable symbolTable) : base(
            labelIlGenerator.GenerateFunc(Types.String, Functions.Substr), Enumerable.Empty<ASTCILExpressionNode>(),
            symbolTable)
        {
        }
    }
}
