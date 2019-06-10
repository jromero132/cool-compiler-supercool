using SuperCOOL.Constants;
using SuperCOOL.Core;
using System.Linq;

namespace SuperCOOL.CodeGeneration.CIL.AST
{
    class ASTCILObjectCopyNode : ASTCILFuncNode
    {
        public ASTCILObjectCopyNode(ILabelILGenerator labelIlGenerator, ISymbolTable symbolTable) : base(
            labelIlGenerator.GenerateFunc(Types.Object, Functions.Copy), Enumerable.Empty<ASTCILExpressionNode>(),
            symbolTable)
        {
        }
    }
}
