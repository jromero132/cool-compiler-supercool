using System.Collections.Generic;
using Antlr4.Runtime;
using SuperCOOL.Core;

namespace SuperCOOL.SemanticCheck.AST
{
    public class ASTMethodNode : ASTNode
    {
        public IToken Method { get; set; }
        public string Name => Method.Text;
        public IToken Return { get; set; }
        public string ReturnType => Return.Text;
        public List<(IToken name,IToken type)> Formals { get; internal set; }
        public ASTExpressionNode Body { get; internal set; }

        public override Result Accept<Result>(ISuperCoolASTVisitor<Result> Visitor)
        {
            return Visitor.VisitMethod(this);
        }
    }
}