using System;
using System.Collections.Generic;
using System.Text;

namespace SuperCOOL.CodeGeneration.CIL.AST
{
    class ASTCILNewNode : ASTCILNode
    {
        public string Type { get; }

        public ASTCILNewNode(string type)
        {
            Type = type;
        }
    }
}
