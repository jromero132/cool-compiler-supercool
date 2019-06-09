using System;
using System.Collections.Generic;
using System.Text;
using SuperCOOL.Constants;
using SuperCOOL.Core;

namespace SuperCOOL.CodeGeneration.CIL.AST
{
    public class ASTCILLessThanTwoVariablesNode : ASTCILExpressionNode
    {
        public ASTCILExpressionNode Left { get; }
        public ASTCILExpressionNode Right { get; }

        public ASTCILLessThanTwoVariablesNode( ASTCILExpressionNode left, ASTCILExpressionNode right, ISymbolTable symbolTable ) : base( symbolTable )
        {
            Left = left;
            Right = right;
        }

        public override Result Accept<Result>( ICILVisitor<Result> Visitor ) => Visitor.VisitLessThanTwoVariables( this );
    }
}
