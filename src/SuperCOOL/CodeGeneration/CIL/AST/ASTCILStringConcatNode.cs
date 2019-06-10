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
    }
}
