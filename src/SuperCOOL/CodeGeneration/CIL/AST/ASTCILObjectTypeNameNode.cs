using SuperCOOL.Constants;
using SuperCOOL.Core.Constants;

namespace SuperCOOL.CodeGeneration.CIL.AST
{
    class ASTCILObjectTypeNameNode : ASTCILFuncNode
    {
        public ASTCILObjectTypeNameNode(ILabelILGenerator labelIlGenerator) : base(
            labelIlGenerator.GenerateFunc(Types.Object, Functions.Type_Name),
            new[] { new ASTCILGetAttrNode(Types.Object, Attributes.TypeName) })
        {
        }
    }
}
