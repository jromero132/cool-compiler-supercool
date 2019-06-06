using System;
using System.Collections.Generic;
using System.Text;
using SuperCOOL.Constants;

namespace SuperCOOL.CodeGeneration.CIL.AST
{
    class ASTCILLessThanTwoVariablesNode : ASTCILExpressionNode
    {
        public ASTCILExpressionNode Left { get; }
        public ASTCILExpressionNode Right { get; }

        public ASTCILLessThanTwoVariablesNode(ASTCILExpressionNode left, ASTCILExpressionNode right) : base(Types.Bool)
        {
            Left = left;
            Right = right;
        }
    }
}
