using System;
using System.Collections.Generic;
using System.Text;
using SuperCOOL.Constants;

namespace SuperCOOL.CodeGeneration.CIL.AST
{
    public class ASTCILBoolNotNode : ASTCILExpressionNode
    {
        public ASTCILExpressionNode Expression { get; }

        public ASTCILBoolNotNode( ASTCILExpressionNode expression ) : base( Types.Bool )
        {
            Expression = expression;
        }

        public override Result Accept<Result>( ICILVisitor<Result> Visitor ) => Visitor.VisitBoolNot( this );
    }
}
