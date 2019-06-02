using SuperCOOL.Core;

namespace SuperCOOL.SemanticCheck.AST
{
    public abstract class ASTNode
    {
        public TypeEnvironment TypeEnvironment { get; set; }
        public SemanticCheckResult SemanticCheckResult { get; set; }
        public virtual Result Accept<Result>(ISuperCoolASTVisitor<Result> Visitor)
        {
            return Visitor.Visit(this);
        }
    }
}