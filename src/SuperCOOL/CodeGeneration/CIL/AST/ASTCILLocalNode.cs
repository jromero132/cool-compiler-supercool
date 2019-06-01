namespace SuperCOOL.CodeGeneration.CIL.AST
{
    class ASTCILLocalNode : ASTCILNode
    {
        public string Name { get; }
        public string Type { get; }

        public ASTCILLocalNode(string name, string type)
        {
            Name = name;
            Type = type;
        }
    }
}
