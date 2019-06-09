﻿using SuperCOOL.Constants;

namespace SuperCOOL.CodeGeneration.CIL.AST
{
  public class ASTCILMultiplyTwoConstantNode : ASTCILExpressionNode
    {
        public int Left { get; }
        public int Right { get; }

        public ASTCILMultiplyTwoConstantNode(int left, int right) : base(Types.Int)
        {
            Left = left;
            Right = right;
        }
    }
}
