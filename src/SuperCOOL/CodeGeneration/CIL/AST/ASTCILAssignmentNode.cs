using SuperCOOL.Core;

namespace SuperCOOL.CodeGeneration.CIL.AST
{
    public class ASTCILAssignmentNode : ASTCILExpressionNode
    {
        public SymbolInfo Identifier { get; }
        public ASTCILExpressionNode Expresion { get; }

        public ASTCILAssignmentNode(SymbolInfo identifier, ASTCILExpressionNode expresion):base()
        {
            Identifier = identifier;
            Expresion = expresion;
        }

        public override Result Accept<Result>( ICILVisitor<Result> Visitor ) => Visitor.VisitAssignment( this );
    }
}
