﻿using SuperCOOL.Core;

namespace SuperCOOL.SemanticCheck.AST
{
    public class ASTEqualNode : ASTExpressionNode
    {
        public ASTExpressionNode Left { get; set; }
        public ASTExpressionNode Right { get; set; }

        public override Result Accept<Result>(ISuperCoolASTVisitor<Result> Visitor)
        {
            return Visitor.VisitEqual(this);
        }
    }
}