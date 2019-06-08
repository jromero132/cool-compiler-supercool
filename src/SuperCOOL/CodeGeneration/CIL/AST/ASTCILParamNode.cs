namespace SuperCOOL.CodeGeneration.CIL.AST
{
    class ASTCILParamNode : ASTCILExpressionNode
    {
        public string Name { get; }

        public ASTCILParamNode(string name, string type) : base(type)
        {
            Name = name;
        }
    }
}
