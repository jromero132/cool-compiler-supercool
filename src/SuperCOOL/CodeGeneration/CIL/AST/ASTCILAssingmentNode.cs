namespace SuperCOOL.CodeGeneration.CIL.AST
{
    class ASTCILAssingmentNode : ASTCILExpressionNode
    {
        public ASTCILIdNode Id { get; }
        public ASTCILExpressionNode Expresion { get; }

        public ASTCILAssingmentNode(ASTCILIdNode id, ASTCILExpressionNode expresion)
        {
            Id = id;
            Expresion = expresion;
        }
    }
}
