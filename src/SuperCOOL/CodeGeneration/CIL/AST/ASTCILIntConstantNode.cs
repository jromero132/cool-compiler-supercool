namespace SuperCOOL.CodeGeneration.CIL.AST
{
    class ASTCILIntConstantNode : ASTCILExpressionNode
    {
        public int Value { get; }

        public ASTCILIntConstantNode(int value)
        {
            Value = value;
        }
    }
}
