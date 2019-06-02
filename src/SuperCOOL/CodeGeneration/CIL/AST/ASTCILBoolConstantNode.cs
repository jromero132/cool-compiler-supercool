using System;
using System.Collections.Generic;
using System.Text;

namespace SuperCOOL.CodeGeneration.CIL.AST
{
    class ASTCILBoolConstantNode : ASTCILExpressionNode
    {
        public bool Value { get; }

        public ASTCILBoolConstantNode(bool value)
        {
            Value = value;
        }
    }
}
