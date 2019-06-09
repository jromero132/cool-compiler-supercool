using System.Collections.Generic;
using System.Linq;

namespace SuperCOOL.CodeGeneration.CIL.AST
{
    public class ASTCILFuncNode : ASTCILNode
    {
        public string Name { get; }
        public IReadOnlyList<ASTCILExpressionNode> Body { get; }

        public ASTCILFuncNode(string name, IEnumerable<ASTCILExpressionNode> body)
        {
            Name = name;
            Body = body.ToList();
        }
    }
}