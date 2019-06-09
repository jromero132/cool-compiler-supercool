using System.Collections.Generic;
using System.Linq;

namespace SuperCOOL.CodeGeneration.CIL.AST
{
    public class ASTCILBlockNode : ASTCILExpressionNode
    {
        public IReadOnlyList<ASTCILExpressionNode> Expressions { get; }

        public ASTCILBlockNode( IEnumerable<ASTCILExpressionNode> expressions )
        {
            Expressions = expressions.ToList();
            Type = Expressions.Last().Type;
        }

        public override Result Accept<Result>( ICILVisitor<Result> Visitor ) => Visitor.VisitBlock( this );
    }
}
