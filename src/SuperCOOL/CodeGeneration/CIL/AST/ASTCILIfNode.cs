namespace SuperCOOL.CodeGeneration.CIL.AST
{
    class ASTCILIfNode : ASTCILNode
    {
        public ASTCILExpressionNode Condition { get; }
        public string Label { get; }

        public ASTCILIfNode(ASTCILExpressionNode condition, string label)
        {
            Condition = condition;
            Label = label;
        }
    }
}
