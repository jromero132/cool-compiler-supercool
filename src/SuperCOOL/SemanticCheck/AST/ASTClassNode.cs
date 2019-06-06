using System;
using System.Collections.Generic;
using System.Text;
using Antlr4.Runtime;
using SuperCOOL.Core;

namespace SuperCOOL.SemanticCheck.AST
{
    public class ASTClassNode:ASTNode
    {
        public IToken Type { get; set; }
        public string TypeName => Type.Text;
        public IToken ParentType { get; set; }
        public string ParentTypeName => ParentType?.Text??"Object";
        internal List<ASTMethodNode> Methods { get; set; }
        internal List<ASTAtributeNode> Atributes { get; set; }

        public override Result Accept<Result>(ISuperCoolASTVisitor<Result> Visitor)
        {
            return Visitor.VisitClass(this);
        }
    }
}
