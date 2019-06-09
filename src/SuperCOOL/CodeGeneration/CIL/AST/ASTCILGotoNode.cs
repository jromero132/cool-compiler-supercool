using SuperCOOL.Core;
using System;
using System.Collections.Generic;
using System.Text;

namespace SuperCOOL.CodeGeneration.CIL.AST
{
    public class ASTCILGotoNode : ASTCILExpressionNode
    {
        public string Label { get; }
        public ASTCILGotoNode( string label,ISymbolTable symbolTable):base(symbolTable)
        {
            Label = label;
        }

        public override Result Accept<Result>( ICILVisitor<Result> Visitor ) => Visitor.VisitGoto( this );
    }
}
