namespace SuperCOOL.CodeGeneration.CIL.AST
{
  public class ASTCILSetAttributeNode : ASTCILExpressionNode
    {
        public string TypeName { get; }
        public string AttributeName { get; }
        public ASTCILExpressionNode Expression { get; }

        public ASTCILSetAttributeNode(string typeName, string attributeName, ASTCILExpressionNode expression)
        {
            TypeName = typeName;
            AttributeName = attributeName;
            Expression = expression;
        }
    }
}
