using System.Collections.Generic;
using System.Linq;

namespace SuperCOOL.CodeGeneration.CIL.AST
{
    class ASTCILFuncNode : ASTCILNode
    {
        public string Name { get; }
        public IReadOnlyList<ASTCILFormalNode> Formals { get; }
        public IReadOnlyList<ASTCILExpressionNode> Body { get; }

        public ASTCILFuncNode(string name, IEnumerable<ASTCILFormalNode> formals, IEnumerable<ASTCILExpressionNode> body)
        {
            Name = name;
            Formals = formals.ToList();
            Body = body.ToList();
        }
    }
}