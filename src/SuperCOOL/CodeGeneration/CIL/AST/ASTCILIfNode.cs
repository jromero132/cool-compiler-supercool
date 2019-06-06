namespace SuperCOOL.CodeGeneration.CIL.AST
{
    class ASTCILIfNode : ASTCILExpressionNode
    {
        public ASTCILExpressionNode Condition { get; }
        public ASTCILExpressionNode Then { get; }
        public ASTCILExpressionNode Else { get; }
        public string Label { get; }

        public ASTCILIfNode(ASTCILExpressionNode condition, ASTCILExpressionNode then, ASTCILExpressionNode @else,
            string label)
        {
            Condition = condition;
            Then = then;
            Else = @else;
            Label = label;
        }
    }
}
