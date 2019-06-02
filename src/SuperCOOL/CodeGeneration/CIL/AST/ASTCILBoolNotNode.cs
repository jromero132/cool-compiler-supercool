using System;
using System.Collections.Generic;
using System.Text;

namespace SuperCOOL.CodeGeneration.CIL.AST
{
    class ASTCILBoolNotNode : ASTCILExpressionNode
    {
        public ASTCILExpressionNode Expression { get; }

        public ASTCILBoolNotNode(ASTCILExpressionNode expression)
        {
            Expression = expression;
        }
    }
}
