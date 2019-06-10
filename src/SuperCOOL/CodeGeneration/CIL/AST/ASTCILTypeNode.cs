using System.Collections.Generic;
using System.Collections.Immutable;
using SuperCOOL.Core;

namespace SuperCOOL.CodeGeneration.CIL.AST
{
    public class ASTCILTypeNode : ASTCILNode
    {
        public int AllocateSize => Attributes.Count * 4;
        public CoolType Type { get; }
        public IReadOnlyList<SymbolInfo> Attributes { get; }
        public IReadOnlyList<CoolMethod> VirtualTable { get; }
        public IReadOnlyList<ASTCILFuncNode> Methods { get; }

        public ASTCILTypeNode( CoolType type, IEnumerable<SymbolInfo> attributes, IEnumerable<CoolMethod> virtualTable,
            IEnumerable<ASTCILFuncNode> methods,ISymbolTable symbolTable):base(symbolTable)
        {
            Type = type;
            Methods = methods.ToImmutableList();
            Attributes = attributes.ToImmutableList();
            VirtualTable = virtualTable.ToImmutableList();
        }

        public override Result Accept<Result>( ICILVisitor<Result> Visitor ) => Visitor.VisitType( this );
    }
}
