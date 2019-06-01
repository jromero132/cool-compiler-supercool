namespace SuperCOOL.CodeGeneration.CIL.AST
{
    class ASTCILStringConstantNode : ASTCILNode
    {
        public string Value { get; }

        public ASTCILStringConstantNode(string value)
        {
            Value = value;
        }
    }
}
