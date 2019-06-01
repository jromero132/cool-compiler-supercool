using System;
using System.Collections.Generic;
using System.Text;

namespace SuperCOOL.CodeGeneration.CIL.AST
{
    class ASTCILGotoNode : ASTCILExpressionNode
    {
        public string Label { get; }
        public ASTCILGotoNode(string label)
        {
            Label = label;
        }
    }
}
