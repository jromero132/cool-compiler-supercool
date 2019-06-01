using System.Collections.Generic;
using SuperCOOL.Core;

namespace SuperCOOL.SemanticCheck.AST
{
    public class ASTProgramNode:ASTNode
    {
        public List<ASTClassNode> Clases { get; set; }
        public ASTProgramNode()
        {
            Clases = new List<ASTClassNode>();
        }
        public override Result Accept<Result>(ISuperCoolASTVisitor<Result> Visitor)
        {
            return Visitor.VisitProgram(this);
        }
    }
}