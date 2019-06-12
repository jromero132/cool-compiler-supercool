using SuperCOOL.Core;
using System.Collections.Generic;
using System.Linq;

namespace SuperCOOL.CodeGeneration.CIL.AST
{
    public class ASTCILFuncVirtualCallNode : ASTCILExpressionNode
    {
        public string MethodName { get; }
        public CoolType Type { get; }
        public IReadOnlyList<ASTCILExpressionNode> Arguments { get; }

        public ASTCILFuncVirtualCallNode(CoolType type ,string methodName, IEnumerable<ASTCILExpressionNode> arguments):base ()
        {
            MethodName = methodName;
            Arguments = arguments.ToList();
            Type = type;
        }

        public override Result Accept<Result>( ICILVisitor<Result> Visitor ) => Visitor.VisitFuncVirtualCall( this );
    }
}
