using SuperCOOL.Core;

namespace SuperCOOL.SemanticCheck.AST
{
    public abstract class ASTNode
    {
        protected ASTNode()
        {
            TypeEnvironment = new TypeEnvironment();
        }
        public TypeEnvironment TypeEnvironment { get; set; }
        public virtual Result Accept<Result>(ISuperCoolASTVisitor<Result> Visitor)
        {
            return Visitor.Visit(this);
        }
    }
}