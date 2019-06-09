using SuperCOOL.Constants;

namespace SuperCOOL.CodeGeneration.CIL.AST
{
    class ASTCILObjectAbortNode : ASTCILFuncNode
    {
        public ASTCILObjectAbortNode(ILabelILGenerator labelIlGenerator) : base(
            labelIlGenerator.GenerateFunc(Types.Object, Functions.Abort),
            new[] { new ASTCILRuntimeErrorNode(RuntimeErrors.ObjectAbort) })
        {
        }
    }
}
