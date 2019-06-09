using SuperCOOL.Core;

namespace SuperCOOL.CodeGeneration.CIL.AST
{
    public class ASTCILAssignmentNode : ASTCILExpressionNode
    {
        public string Identifier { get; }
        public ASTCILExpressionNode Expresion { get; }

        public ASTCILAssignmentNode( string identifier, ASTCILExpressionNode expresion,ISymbolTable symbolTable):base(symbolTable)
        {
            Identifier = identifier;
            Expresion = expresion;
        }

        public override Result Accept<Result>( ICILVisitor<Result> Visitor ) => Visitor.VisitAssignment( this );
    }
}
