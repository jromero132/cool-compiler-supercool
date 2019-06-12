using SuperCOOL.Core;

namespace SuperCOOL.CodeGeneration.CIL.AST
{
    public class ASTCILGetAttrNode : ASTCILExpressionNode
    {
        public SymbolInfo Atribute { get; set; }

        public ASTCILGetAttrNode(SymbolInfo Atribute) :base()
        {
            this.Atribute = Atribute;
        }

        public override Result Accept<Result>( ICILVisitor<Result> Visitor ) => Visitor.VisitGetAttr( this );
    }
}
