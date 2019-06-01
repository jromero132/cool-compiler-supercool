namespace SuperCOOL.CodeGeneration.CIL.AST
{
    class ASTCILProgramNode : ASTCILNode
    {
        public ASTCILCodeNode Code { get; }
        public ASTCILDataNode Data { get; }

        public ASTCILProgramNode(ASTCILCodeNode code, ASTCILDataNode data)
        {
            Code = code;
            Data = data;
        }
    }
}
