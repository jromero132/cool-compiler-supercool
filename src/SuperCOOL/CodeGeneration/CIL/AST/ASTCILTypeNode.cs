using System.Collections.Generic;
using System.Collections.Immutable;
using SuperCOOL.Core;

namespace SuperCOOL.CodeGeneration.CIL.AST
{
  public class ASTCILTypeNode : ASTCILNode
    {
        public int AllocateSize => Attributes.Count * 4;
        public CoolType Type { get; }
        public IReadOnlyList<SymbollInfo> Attributes { get; }
        public IReadOnlyList<CoolMethod> VirtualTable { get; }
        public IReadOnlyList<ASTCILFuncNode> Methods { get; }

        public ASTCILTypeNode(CoolType type, IEnumerable<SymbollInfo> attributes, IEnumerable<CoolMethod> virtualTable,
            IEnumerable<ASTCILFuncNode> methods)
        {
            Type = type;
            Methods = methods.ToImmutableList();
            Attributes = attributes.ToImmutableList();
            VirtualTable = virtualTable.ToImmutableList();
        }
    }
}
