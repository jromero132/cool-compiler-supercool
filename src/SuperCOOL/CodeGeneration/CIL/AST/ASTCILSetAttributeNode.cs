using SuperCOOL.Core;

namespace SuperCOOL.CodeGeneration.CIL.AST
{
    public class ASTCILSetAttributeNode : ASTCILExpressionNode
    {
        public SymbolInfo Atribute { get; set; }
        public ASTCILExpressionNode Expression { get; }

        public ASTCILSetAttributeNode(SymbolInfo Atribute, ASTCILExpressionNode expression):base()
        {
            this.Atribute = Atribute;
            Expression = expression;
        }

        public override Result Accept<Result>( ICILVisitor<Result> Visitor ) => Visitor.VisitSetAttribute( this );
    }
}
