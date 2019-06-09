using System.Collections.Generic;
using System.Linq;

namespace SuperCOOL.CodeGeneration.CIL.AST
{
    public class ASTCILFuncVirtualCallNode : ASTCILExpressionNode
    {
        public string MethodName { get; }
        public IReadOnlyList<ASTCILExpressionNode> Arguments { get; }

        public ASTCILFuncVirtualCallNode( string methodName, IEnumerable<ASTCILExpressionNode> arguments )
        {
            MethodName = methodName;
            Arguments = arguments.ToList();
        }

        public override Result Accept<Result>( ICILVisitor<Result> Visitor ) => Visitor.VisitFuncVirtualCall( this );
    }
}
