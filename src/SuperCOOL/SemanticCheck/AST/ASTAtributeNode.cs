
using Antlr4.Runtime;
using SuperCOOL.Core;

namespace SuperCOOL.SemanticCheck.AST
{
    public class ASTAtributeNode : ASTNode
    {
        public IToken Attribute { get; internal set; }
        public string AttributeName => Attribute.Text;
        public IToken Type { get; internal set; }
        public string TypeName { get; internal set; }
        public ASTExpressionNode Init { get; internal set; }
        public bool HasInit => Init != null;
        public override Result Accept<Result>(ISuperCoolASTVisitor<Result> Visitor)
        {
            return Visitor.VisitAtribute(this);
        }
    }
}