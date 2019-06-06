namespace SuperCOOL.CodeGeneration.CIL.AST
{
    abstract class ASTCILExpressionNode : ASTCILNode
    {
        public ASTCILExpressionNode()
        {
            
        }
        public ASTCILExpressionNode(string type)
        {
            Type = type;
        }
        public string Type { get; protected internal set; }
    }
}
