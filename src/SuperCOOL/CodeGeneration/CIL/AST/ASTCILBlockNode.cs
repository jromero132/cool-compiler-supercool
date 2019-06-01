using System.Collections.Generic;
using System.Linq;

namespace SuperCOOL.CodeGeneration.CIL.AST
{
    class ASTCILBlockNode : ASTCILExpressionNode
    {
        public IReadOnlyList<ASTCILExpressionNode> Expressions { get; }

        public ASTCILBlockNode(IEnumerable<ASTCILExpressionNode> expressions)
        {
            Expressions = expressions.ToList();
        }
    }
}
