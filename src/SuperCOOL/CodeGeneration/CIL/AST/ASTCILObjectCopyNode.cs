using System.Collections.Generic;
using System.Linq;
using SuperCOOL.Constants;

namespace SuperCOOL.CodeGeneration.CIL.AST
{
    class ASTCILObjectCopyNode : ASTCILFuncNode
    {
        public ASTCILObjectCopyNode(ILabelILGenerator labelIlGenerator) : base(
            labelIlGenerator.GenerateFunc(Types.Object, Functions.Copy), new[]
            {
                new ASTCILNewNode(""), //
            })
        {
        }
    }
}
