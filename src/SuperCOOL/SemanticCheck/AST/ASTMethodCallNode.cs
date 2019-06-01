﻿using SuperCOOL.Core;

namespace SuperCOOL.SemanticCheck.AST
{
    public class ASTStaticMethodCallNode:ASTExpressionNode
    {
        public string MethodName { get; internal set; }
        public string Type { get; internal set; }
        internal ASTExpressionNode InvokeOnExpresion { get; set; }
        internal ASTExpressionNode[] Arguments { get; set; }

        public override Result Accept<Result>(ISuperCoolASTVisitor<Result> Visitor)
        {
            return Visitor.VisitStaticMethodCall(this);
        }
    }
}