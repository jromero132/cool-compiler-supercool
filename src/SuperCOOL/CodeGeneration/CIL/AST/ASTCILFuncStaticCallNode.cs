using SuperCOOL.Core;
using System.Collections.Generic;
using System.Linq;

namespace SuperCOOL.CodeGeneration.CIL.AST
{
    public class ASTCILFuncStaticCallNode : ASTCILExpressionNode
    {
        public string Type { get; }
        public string MethodName { get; }
        public IReadOnlyList<ASTCILExpressionNode> Arguments { get; }

        public ASTCILFuncStaticCallNode( string methodName, string type, IEnumerable<ASTCILExpressionNode> arguments,ISymbolTable symbolTable) : base(symbolTable )
        {
            this.Type= type;
            MethodName = methodName;
            Arguments = arguments.ToList();
        }

        public override Result Accept<Result>( ICILVisitor<Result> Visitor ) => Visitor.VisitFuncStaticCall( this );
    }
}
