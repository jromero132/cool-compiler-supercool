namespace SuperCOOL.CodeGeneration.CIL.AST
{
    class ASTCILAssingmentNode : ASTCILExpressionNode
    {
        public string Identifier { get; }
        public ASTCILExpressionNode Expresion { get; }

        public ASTCILAssingmentNode(string identifier, ASTCILExpressionNode expresion)
        {
            Identifier = identifier;
            Expresion = expresion;
        }
    }
}
