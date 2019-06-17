using SuperCOOL.Core;
using System.Collections.Generic;
using System.Collections.Immutable;

namespace SuperCOOL.CodeGeneration.CIL.AST
{
    public class ASTCILCaseNode : ASTCILExpressionNode
    {
        public ASTCILExpressionNode ExpressionCase { get; }
        public IReadOnlyList<(CoolType type, ASTCILExpressionNode expression)> Cases { get; }

        public ASTCILCaseNode(ASTCILExpressionNode expressionCase, IEnumerable<(CoolType type, ASTCILExpressionNode expression)> cases)
        {
            ExpressionCase = expressionCase;
            Cases = cases.ToImmutableList();
        }

        public override Result Accept<Result>(ICILVisitor<Result> Visitor) => Visitor.VisitCase(this);
    }
}
