﻿using SuperCOOL.Constants;

namespace SuperCOOL.CodeGeneration.CIL.AST
{
    public class ASTCILIntConstantNode : ASTCILExpressionNode
    {
        public int Value { get; }

        public ASTCILIntConstantNode(int value) : base(Types.Int)
        {
            Value = value;
        }
    }
}
