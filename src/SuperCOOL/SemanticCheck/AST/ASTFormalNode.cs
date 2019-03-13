
using SuperCOOL.Core;

namespace SuperCOOL.SemanticCheck.AST
{
    public class ASTFormalNode : ASTNode
    {
        public string Name { get; internal set; }
        public string Type { get; internal set; }

        public override Result Accept<Result>(ISuperCoolASTVisitor<Result> Visitor)
        {
            return Visitor.VisitFormal(this);
        }
    }
}