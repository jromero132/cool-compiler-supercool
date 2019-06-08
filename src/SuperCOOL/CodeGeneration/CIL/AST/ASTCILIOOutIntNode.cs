using System;
using System.Linq;

namespace SuperCOOL.CodeGeneration.CIL.AST
{
    class ASTCILIOOutIntNode : ASTCILFuncNode
    {
        public int Value { get; }

        public ASTCILIOOutIntNode(int value) : base("", // TODO set func name 
            Enumerable.Empty<ASTCILExpressionNode>())
        {
            Value = value;
            throw new NotImplementedException();
        }
    }
}
