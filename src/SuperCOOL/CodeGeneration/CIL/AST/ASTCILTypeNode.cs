using System.Collections.Generic;
using System.Collections.Immutable;
using SuperCOOL.Core;

namespace SuperCOOL.CodeGeneration.CIL.AST
{
    public class ASTCILTypeNode : ASTCILNode
    {
        public CoolType Type { get; }
        public IReadOnlyList<CoolMethod> VirtualTable { get; }
        public IReadOnlyList<ASTCILFuncNode> Methods { get; }

        public ASTCILTypeNode( CoolType type, IEnumerable<CoolMethod> virtualTable,
            IEnumerable<ASTCILFuncNode> methods):base()
        {
            Type = type;
            Methods = methods.ToImmutableList();
            VirtualTable = virtualTable.ToImmutableList();
        }

        public override Result Accept<Result>( ICILVisitor<Result> Visitor ) => Visitor.VisitType( this );
    }
}
