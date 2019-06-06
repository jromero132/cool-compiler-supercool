using System;
using System.Collections.Generic;
using System.Text;
using SuperCOOL.Constants;

namespace SuperCOOL.CodeGeneration.CIL.AST
{
    class ASTCILBoolNotNode : ASTCILExpressionNode
    {
        public ASTCILExpressionNode Expression { get; }

        public ASTCILBoolNotNode(ASTCILExpressionNode expression) : base(Types.Bool)
        {
            Expression = expression;
        }
    }
}
