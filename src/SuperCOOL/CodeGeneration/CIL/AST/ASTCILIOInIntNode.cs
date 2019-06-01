using System;
using System.Linq;

namespace SuperCOOL.CodeGeneration.CIL.AST
{
    class ASTCILIOInIntNode : ASTCILFuncNode
    {
        public ASTCILIOInIntNode() : base("", Enumerable.Empty<ASTCILFormalNode>(), // TODO set func name
            Enumerable.Empty<ASTCILExpressionNode>(), Enumerable.Empty<ASTCILLocalNode>())
        {
            throw new NotImplementedException();
        }
    }
}
