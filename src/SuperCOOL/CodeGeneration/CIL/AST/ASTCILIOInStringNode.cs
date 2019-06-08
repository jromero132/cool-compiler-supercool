using System;
using System.Linq;

namespace SuperCOOL.CodeGeneration.CIL.AST
{
    class ASTCILIOInStringNode : ASTCILFuncNode
    {
        public ASTCILIOInStringNode() : base("", // TODO set func name
            Enumerable.Empty<ASTCILExpressionNode>())
        {
            throw new NotImplementedException();
        }
    }
}
