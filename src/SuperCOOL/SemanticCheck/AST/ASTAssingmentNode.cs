using Antlr4.Runtime;
using SuperCOOL.Core;

namespace SuperCOOL.SemanticCheck.AST
{
    public class ASTAssingmentNode : ASTExpressionNode
    {
        public ASTIdNode Id { get; internal set; }
        public ASTExpressionNode Expresion { get; set; }
        public IToken AssigmentToken { get; internal set; }

        public override Result Accept<Result>(ISuperCoolASTVisitor<Result> Visitor)
        {
            return Visitor.VisitAssignment(this);
        }
    }
}