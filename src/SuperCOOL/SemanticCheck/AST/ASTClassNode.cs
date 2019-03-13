using System;
using System.Collections.Generic;
using System.Text;
using SuperCOOL.Core;

namespace SuperCOOL.SemanticCheck.AST
{
    public class ASTClassNode:ASTNode
    {
        public string TypeName { get; internal set; }
        public string ParentTypeName { get; internal set; }
        internal List<ASTMethodNode> Methods { get; set; }
        internal List<ASTAtributeNode> Atributes { get; set; }

        public override Result Accept<Result>(ISuperCoolASTVisitor<Result> Visitor)
        {
            return Visitor.VisitClass(this);
        }
    }
}
