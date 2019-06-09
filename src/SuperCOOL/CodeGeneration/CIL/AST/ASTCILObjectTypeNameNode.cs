using SuperCOOL.Constants;
using SuperCOOL.Core;
using SuperCOOL.Core.Constants;

namespace SuperCOOL.CodeGeneration.CIL.AST
{
    class ASTCILObjectTypeNameNode : ASTCILFuncNode
    {
        public ASTCILObjectTypeNameNode(ILabelILGenerator labelIlGenerator, ISymbolTable symbolTable) : base(
            labelIlGenerator.GenerateFunc(Types.Object, Functions.Type_Name),
            new[] { new ASTCILGetAttrNode(Types.Object, Attributes.TypeName, symbolTable) }, symbolTable)
        {
        }
    }
}
