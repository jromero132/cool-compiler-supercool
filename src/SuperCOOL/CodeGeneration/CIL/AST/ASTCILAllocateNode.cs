namespace SuperCOOL.CodeGeneration.CIL.AST
{
    public class ASTCILAllocateNode : ASTCILExpressionNode
    {
        public ASTCILLocalNode Variable { get; }
        public ASTCILAllocateNode(string type, ASTCILLocalNode variable) : base(type)
        {
            Variable = variable;
        }
    }
}
