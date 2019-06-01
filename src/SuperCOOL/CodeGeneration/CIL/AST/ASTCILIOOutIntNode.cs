using System;
using System.Linq;

namespace SuperCOOL.CodeGeneration.CIL.AST
{
    class ASTCILIOOutIntNode : ASTCILFuncNode
    {
        public int Value { get; }

        public ASTCILIOOutIntNode(int value) : base("", // TODO set func name 
            Enumerable.Repeat(new ASTCILFormalNode("value", ""), 1), //TODO set param name and type name int
            Enumerable.Empty<ASTCILExpressionNode>(), Enumerable.Empty<ASTCILLocalNode>())
        {
            Value = value;
            throw new NotImplementedException();
        }
    }
}
