using System.Collections.Generic;

namespace SuperCOOL.CodeGeneration.CIL.AST
{
    class ASTCILTypeNode
    {
        public int AllocateSize { get; }
        public IReadOnlyDictionary<string, int> AttributesOffset { get; }
        public IReadOnlyDictionary<string, int> MethodsOffset { get; }

        public ASTCILTypeNode(int allocateSize, IReadOnlyDictionary<string, int> attributesOffset,
            IReadOnlyDictionary<string, int> methodsOffset)
        {
            AllocateSize = allocateSize;
            AttributesOffset = new Dictionary<string, int>(attributesOffset);
            MethodsOffset = new Dictionary<string, int>(methodsOffset);
        }
    }
}
