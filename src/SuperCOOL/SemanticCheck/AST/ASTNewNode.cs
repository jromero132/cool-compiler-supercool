using Antlr4.Runtime;
using SuperCOOL.Core;

namespace SuperCOOL.SemanticCheck.AST
{
    public class ASTNewNode:ASTExpressionNode
    {
        public IToken Type { get; internal set; }
        public string TypeName => Type.Text;

        public override Result Accept<Result>(ISuperCoolASTVisitor<Result> Visitor)
        {
            return Visitor.VisitNew(this);
        }
    }
}