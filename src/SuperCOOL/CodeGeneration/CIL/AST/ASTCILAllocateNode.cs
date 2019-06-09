using SuperCOOL.Core;

namespace SuperCOOL.CodeGeneration.CIL.AST
{
    public class ASTCILAllocateNode : ASTCILExpressionNode
    {
        public string Type { get; }

        public ASTCILAllocateNode(string type,ISymbolTable symbolTable) : base( symbolTable )
        {
            this.Type = type;
        }

        public override Result Accept<Result>( ICILVisitor<Result> Visitor ) => Visitor.VisitAllocate( this );
    }
}
