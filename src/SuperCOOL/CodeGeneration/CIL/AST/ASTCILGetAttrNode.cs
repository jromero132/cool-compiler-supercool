namespace SuperCOOL.CodeGeneration.CIL.AST
{
    class ASTCILGetAttrNode : ASTCILExpressionNode
    {
        public string TypeName { get; }
        public string AttributeName { get; }

        public ASTCILGetAttrNode(string typeName, string attributeName)
        {
            TypeName = typeName;
            AttributeName = attributeName;
        }
    }
}
