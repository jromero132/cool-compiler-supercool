﻿using SuperCOOL.Core;

namespace SuperCOOL.CodeGeneration.CIL.AST
{
    public class ASTCILBoolOrTwoConstantNode : ASTCILExpressionNode
    {
        public bool Left { get; }
        public bool Right { get; }

        public ASTCILBoolOrTwoConstantNode( bool left, bool right,ISymbolTable symbolTable):base(symbolTable)
        {
            Left = left;
            Right = right;
        }

        public override Result Accept<Result>( ICILVisitor<Result> Visitor ) => Visitor.VisitBoolOrTwoConstant( this );
    }
}
