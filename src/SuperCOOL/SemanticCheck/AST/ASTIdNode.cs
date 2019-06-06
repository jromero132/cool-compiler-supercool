using Antlr4.Runtime;
using SuperCOOL.Core;

namespace SuperCOOL.SemanticCheck.AST
{
    public class ASTIdNode : ASTExpressionNode
    {
        public IToken Token { get; set; }
        public string Name => Token.Text;
        public override Result Accept<Result>(ISuperCoolASTVisitor<Result> Visitor)
        {
            return Visitor.VisitId(this);
        }
    }
}