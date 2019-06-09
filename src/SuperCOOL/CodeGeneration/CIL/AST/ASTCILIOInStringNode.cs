using System;
using System.Linq;
using SuperCOOL.Constants;

namespace SuperCOOL.CodeGeneration.CIL.AST
{
    class ASTCILIOInStringNode : ASTCILFuncNode
    {
        public ASTCILIOInStringNode(ILabelILGenerator labelIlGenerator) : base(labelIlGenerator.GenerateFunc(Types.IO,Functions.InString),
            Enumerable.Empty<ASTCILExpressionNode>())
        {
            throw new NotImplementedException();
        }
    }
}
