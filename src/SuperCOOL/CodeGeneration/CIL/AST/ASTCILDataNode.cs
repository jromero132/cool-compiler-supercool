using System.Collections.Generic;
using System.Linq;

namespace SuperCOOL.CodeGeneration.CIL.AST
{
    class ASTCILDataNode : ASTCILNode
    {
        public IReadOnlyList<ASTCILTypeNode> Types { get; }

        public ASTCILDataNode(IEnumerable<ASTCILTypeNode> types)
        {
            Types = types.ToList();
        }
        // TODO add list null and others data 
    }
}
