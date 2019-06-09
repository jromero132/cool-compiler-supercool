using System.Collections.Generic;
using System.Linq;
using SuperCOOL.Core;

namespace SuperCOOL.CodeGeneration.CIL.AST
{
    public class ASTCILFuncNode : ASTCILNode
    {
        public string Name { get; }
        public IReadOnlyList<ASTCILExpressionNode> Body { get; }

        public ASTCILFuncNode( string name, IEnumerable<ASTCILExpressionNode> body, ISymbolTable symbolTable ) : base(symbolTable)
        {
            Name = name;
            Body = body.ToList();
        }

        public override Result Accept<Result>( ICILVisitor<Result> Visitor ) => Visitor.VisitFunc( this );
    }
}