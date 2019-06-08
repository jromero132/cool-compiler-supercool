using System.Collections.Generic;
using System.Linq;

namespace SuperCOOL.CodeGeneration.CIL.AST
{
    class ASTCILFuncStaticCallNode : ASTCILExpressionNode
    {
        public string MethodName { get; }
        public IReadOnlyList<ASTCILExpressionNode> Arguments { get; }

        public ASTCILFuncStaticCallNode(string methodName, string type, IEnumerable<ASTCILExpressionNode> arguments) : base(type)
        {
            MethodName = methodName;
            Arguments = arguments.ToList();
        }
    }
}
