using SuperCOOL.Core;

namespace SuperCOOL.SemanticCheck.AST
{
    public abstract class ASTNode
    {
        public ASTNode()
        {
            SemanticCheckResult = new SemanticCheckResult();
        }
        public ISymbolTable SymbolTable { get; set; }
        public SemanticCheckResult SemanticCheckResult { get; set; }
        public virtual Result Accept<Result>(ISuperCoolASTVisitor<Result> Visitor)
        {
            return Visitor.Visit(this);
        }
    }
}