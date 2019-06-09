using System;
using System.Linq;
using SuperCOOL.Constants;

namespace SuperCOOL.CodeGeneration.CIL.AST
{
    class ASTCILIOOutIntNode : ASTCILFuncNode
    {
        public ASTCILIOOutIntNode(ILabelILGenerator labelIlGenerator, ASTCILExpressionNode expresion) : base(labelIlGenerator.GenerateFunc(Types.IO,Functions.OutInt),
            new[] { expresion})
        {
        }
    }

}
