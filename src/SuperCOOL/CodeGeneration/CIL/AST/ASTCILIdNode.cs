using SuperCOOL.Core;

namespace SuperCOOL.CodeGeneration.CIL.AST
{
    public class ASTCILIdNode : ASTCILExpressionNode
    {
        public SymbolInfo Name { get; }

        public ASTCILIdNode(SymbolInfo Name) :base()
        {
            this.Name = Name;
        }

        public override Result Accept<Result>( ICILVisitor<Result> Visitor ) => Visitor.VisitId( this );
    }
}
