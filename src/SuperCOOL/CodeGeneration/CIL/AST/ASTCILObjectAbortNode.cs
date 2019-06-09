using SuperCOOL.Constants;
using SuperCOOL.Core;

namespace SuperCOOL.CodeGeneration.CIL.AST
{
    class ASTCILObjectAbortNode : ASTCILFuncNode
    {
        public ASTCILObjectAbortNode(ILabelILGenerator labelIlGenerator, ISymbolTable symbolTable) : base(
            labelIlGenerator.GenerateFunc(Types.Object, Functions.Abort),
            new[] { new ASTCILRuntimeErrorNode(RuntimeErrors.ObjectAbort, symbolTable) }, symbolTable)
        {
        }
    }
}
