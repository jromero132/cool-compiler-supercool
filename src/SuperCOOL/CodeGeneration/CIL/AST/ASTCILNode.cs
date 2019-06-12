using SuperCOOL.Core;
using System.Collections.Generic;

namespace SuperCOOL.CodeGeneration.CIL.AST
{
    public abstract class ASTCILNode
    {
        public static implicit operator List<ASTCILNode>(ASTCILNode node) => new List<ASTCILNode>{node};
        public virtual Result Accept<Result>( ICILVisitor<Result> Visitor ) => Visitor.VisitNode( this );
    }
}
