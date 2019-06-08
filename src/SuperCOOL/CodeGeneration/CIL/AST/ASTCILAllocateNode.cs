namespace SuperCOOL.CodeGeneration.CIL.AST
{
    class ASTCILAllocateNode : ASTCILExpressionNode
    {
        public ASTCILLocalNode Variable { get; }
        public ASTCILAllocateNode(string type, ASTCILLocalNode variable) : base(type)
        {
            Variable = variable;
        }
    }
}
