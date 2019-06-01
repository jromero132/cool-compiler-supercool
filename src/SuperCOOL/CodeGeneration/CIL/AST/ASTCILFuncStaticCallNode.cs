using System.Collections.Generic;
using System.Linq;

namespace SuperCOOL.CodeGeneration.CIL.AST
{
    class ASTCILFuncStaticCallNode : ASTCILExpressionNode
    {
        public string MethodName { get; }
        public string Type { get; }
        public IReadOnlyList<ASTCILExpressionNode> Arguments { get; }

        public ASTCILFuncStaticCallNode(string methodName, string type, IEnumerable<ASTCILExpressionNode> arguments)
        {
            MethodName = methodName;
            Type = type;
            Arguments = arguments.ToList();
        }
    }
}
