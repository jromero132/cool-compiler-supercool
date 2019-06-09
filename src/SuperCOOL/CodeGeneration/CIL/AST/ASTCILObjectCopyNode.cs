using System.Collections.Generic;
using System.Linq;
using SuperCOOL.Constants;
using SuperCOOL.Core;

namespace SuperCOOL.CodeGeneration.CIL.AST
{
    class ASTCILObjectCopyNode : ASTCILFuncNode
    {
        public ASTCILObjectCopyNode(ILabelILGenerator labelIlGenerator, ISymbolTable symbolTable) : base(
            labelIlGenerator.GenerateFunc(Types.Object, Functions.Copy), new[]
            {
                new ASTCILNewNode("",symbolTable), //TODO implement
            },symbolTable)
        {
        }
    }
}
