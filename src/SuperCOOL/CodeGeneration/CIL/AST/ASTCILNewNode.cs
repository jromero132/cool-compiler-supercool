using System;
using System.Collections.Generic;
using System.Text;

namespace SuperCOOL.CodeGeneration.CIL.AST
{
    public class ASTCILNewNode : ASTCILNode
    {
        public string Type { get; }

        public ASTCILNewNode( string type )
        {
            Type = type;
        }

        public override Result Accept<Result>( ICILVisitor<Result> Visitor ) => Visitor.VisitNew( this );
    }
}
