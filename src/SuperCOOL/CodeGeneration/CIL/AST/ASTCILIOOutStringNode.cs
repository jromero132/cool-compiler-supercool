using System;
using System.Linq;

namespace SuperCOOL.CodeGeneration.CIL.AST
{
    class ASTCILIOOutStringNode : ASTCILFuncNode
    {
        public string Value { get; }

        public ASTCILIOOutStringNode(string value) : base("", // TODO set func name 
            Enumerable.Repeat(new ASTCILFormalNode("value", ""), 1), //TODO set param name and type name string
            Enumerable.Empty<ASTCILExpressionNode>(), Enumerable.Empty<ASTCILLocalNode>())
        {
            Value = value;
            throw new NotImplementedException();
        }
    }
}
