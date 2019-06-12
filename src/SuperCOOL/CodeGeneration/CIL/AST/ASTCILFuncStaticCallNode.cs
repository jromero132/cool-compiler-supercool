using SuperCOOL.Core;
using System.Collections.Generic;
using System.Linq;

namespace SuperCOOL.CodeGeneration.CIL.AST
{
    public class ASTCILFuncStaticCallNode : ASTCILExpressionNode
    {
        public CoolType Type { get; }
        public string MethodName { get; }
        public IReadOnlyList<ASTCILExpressionNode> Arguments { get; }

        public ASTCILFuncStaticCallNode( string methodName, CoolType type, IEnumerable<ASTCILExpressionNode> arguments) : base()
        {
            this.Type= type;
            MethodName = methodName;
            Arguments = arguments.ToList();
        }

        public override Result Accept<Result>( ICILVisitor<Result> Visitor ) => Visitor.VisitFuncStaticCall( this );
    }
}
