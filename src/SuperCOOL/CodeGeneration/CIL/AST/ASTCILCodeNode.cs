using System.Collections.Generic;

namespace SuperCOOL.CodeGeneration.CIL.AST
{
    class ASTCILCodeNode : ASTCILNode
    {
        public IReadOnlyList<ASTCILFuncNode> Functions { get; }

        public ASTCILCodeNode(IReadOnlyList<ASTCILFuncNode> functions)
        {
            Functions = functions;
        }
        //TODO add default method to Object, String, IO, GarbajeCollector
    }
}
