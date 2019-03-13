using System.Collections.Generic;
using SuperCOOL.Core;

namespace SuperCOOL.SemanticCheck.AST
{
    public class ASTMethodNode : ASTNode
    {
        public string Name { get; internal set; }
        public string ReturnType { get; internal set; }
        public List<ASTFormalNode> Formals { get; internal set; }
        public ASTExpressionNode Body { get; internal set; }

        public override Result Accept<Result>(ISuperCoolASTVisitor<Result> Visitor)
        {
            return Visitor.VisitMethod(this);
        }
    }
}