using System;
using System.Linq;
using SuperCOOL.Constants;

namespace SuperCOOL.CodeGeneration.CIL.AST
{
    class ASTCILIOInIntNode : ASTCILFuncNode
    {
        public ASTCILIOInIntNode(ILabelILGenerator labelgenerator) : base(labelgenerator.GenerateFunc(Types.IO,Functions.InInt),
            Enumerable.Empty<ASTCILExpressionNode>())
        {
        }
    }
}
