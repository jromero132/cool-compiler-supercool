namespace SuperCOOL.CodeGeneration.CIL.AST
{
    class ASTCILFormalNode : ASTCILNode
    {
        public string Name { get; }
        public string Type { get; }

        public ASTCILFormalNode(string name, string type)
        {
            Name = name;
            Type = type;
        }
    }
}
