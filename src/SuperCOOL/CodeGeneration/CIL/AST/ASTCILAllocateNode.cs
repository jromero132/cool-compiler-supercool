using SuperCOOL.Core;

namespace SuperCOOL.CodeGeneration.CIL.AST
{
    public class ASTCILAllocateNode : ASTCILExpressionNode
    {
        public CoolType Type { get; }

        public ASTCILAllocateNode(CoolType type) : base( )
        {
            this.Type = type;
        }

        public override Result Accept<Result>( ICILVisitor<Result> Visitor ) => Visitor.VisitAllocate( this );
    }
}
