using System.Collections.Generic;
using System.Linq;

namespace SuperCOOL.CodeGeneration.CIL.AST
{
    class ASTCILFuncNode : ASTCILNode
    {
        public string Name { get; }
        public IReadOnlyList<ASTCILExpressionNode> Body { get; }
        public IReadOnlyList<ASTCILLocalNode> Locals { get; }

        public ASTCILFuncNode(string name, IEnumerable<ASTCILExpressionNode> body, IEnumerable<ASTCILLocalNode> locals)
        {
            Name = name;
            Locals = locals.ToList();
            Body = body.ToList();
        }
    }
}