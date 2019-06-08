using System.Collections.Generic;
using System.Linq;

namespace SuperCOOL.CodeGeneration.CIL.AST
{
    class ASTCILFuncVirtualCallNode : ASTCILExpressionNode
    {
        public string MethodName { get; }
        public IReadOnlyList<ASTCILExpressionNode> Arguments { get; }

        public ASTCILFuncVirtualCallNode(string methodName, IEnumerable<ASTCILExpressionNode> arguments)
        {
            MethodName = methodName;
            Arguments = arguments.ToList();
        }
    }
}
