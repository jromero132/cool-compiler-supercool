using System;
using System.Collections.Generic;
using System.Text;

namespace SuperCOOL.CodeGeneration.CIL.AST
{
    public class ASTCILSelfNode : ASTCILExpressionNode
    {
        public override Result Accept<Result>( ICILVisitor<Result> Visitor ) => Visitor.VisitSelf( this );
    }
}
