using System;
using System.Linq;

namespace SuperCOOL.CodeGeneration.CIL.AST
{
    class ASTCILIOOutStringNode : ASTCILFuncNode
    {
        public string Value { get; }

        public ASTCILIOOutStringNode(string value) : base("", // TODO set func name 
            Enumerable.Empty<ASTCILExpressionNode>(), Enumerable.Empty<ASTCILLocalNode>())
        {
            Value = value;
            throw new NotImplementedException();
        }
    }
}
