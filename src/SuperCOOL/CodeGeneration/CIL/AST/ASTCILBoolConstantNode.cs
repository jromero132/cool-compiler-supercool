using System;
using System.Collections.Generic;
using System.Text;
using SuperCOOL.Constants;

namespace SuperCOOL.CodeGeneration.CIL.AST
{
    public class ASTCILBoolConstantNode : ASTCILExpressionNode
    {
        public bool Value { get; }

        public ASTCILBoolConstantNode(bool value) : base(Types.Bool)
        {
            Value = value;
        }
    }
}
