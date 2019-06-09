using SuperCOOL.Core;

namespace SuperCOOL.CodeGeneration.CIL.AST
{
    public abstract class ASTCILNode
    {
        public ASTCILNode(ISymbolTable symbolTable)
        {
            SymbolTable = symbolTable;
        }
        public ISymbolTable SymbolTable { get; set; } 
        public virtual Result Accept<Result>( ICILVisitor<Result> Visitor ) => Visitor.VisitNode( this );
    }
}
