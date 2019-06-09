using System.Collections.Generic;
using System.Collections.Immutable;

namespace SuperCOOL.CodeGeneration.CIL.AST
{
    public class ASTCILProgramNode : ASTCILNode
    {
        public IReadOnlyList<ASTCILTypeNode> Types { get; }

        public ASTCILProgramNode( IEnumerable<ASTCILTypeNode> types )
        {
            Types = types.ToImmutableList();
        }

        public override Result Accept<Result>( ICILVisitor<Result> Visitor ) => Visitor.VisitProgram( this );
    }
}
