namespace SuperCOOL.CodeGeneration.CIL.AST
{
    class ASTCILIdNode : ASTCILNode
    {
        public string Name { get; }

        public ASTCILIdNode(string name)
        {
            Name = name;
        }
    }
}
