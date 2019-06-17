using System;
using System.Collections.Generic;
using System.Text;

namespace SuperCOOL.CodeGeneration.CIL.AST
{
    public class ASTCILEqualStringNode : ASTCILExpressionNode
    {
        public ASTCILEqualStringNode(ASTCILExpressionNode left, ASTCILExpressionNode right)
        {
            Left = left;
            Right = right;
        }

        public ASTCILExpressionNode Left { get; }
        public ASTCILExpressionNode Right { get; }
    }
}
